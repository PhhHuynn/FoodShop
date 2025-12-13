using ASM.Server.Data;
using ASM.Server.DTOs.MessageDtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ASM.Server.Hubs
{
	public class ChatHub : Hub
	{
		private readonly AppDbContext _context;

		public ChatHub(AppDbContext context)
		{
			_context = context;
		}
		public async Task SendMessage(MessageCreateDto model)
		{
			try
			{
				var exists = await _context.Conversations.AnyAsync(c => c.Id == model.ConversationId);
				if (!exists)
				{
					await Clients.Caller.SendAsync("Error", "Conversation not found");
					return;
				}


				var message = new Models.Message
				{
					Content = model.Content,
					ConversationId = model.ConversationId,
					SenderId = model.SenderId,
					SenderType = model.SenderType,
					CreateAt = DateTime.UtcNow
				};

				_context.Messages.Add(message);
				await _context.SaveChangesAsync();

				await _context.Conversations
					.Where(c => c.Id == model.ConversationId)
					.ExecuteUpdateAsync(c => c.SetProperty(c => c.UpdatedAt, DateTime.UtcNow));

				var sender = await _context.Users
					.Where(u => u.Id == message.SenderId)
					.Select(u => new { u.FullName })
					.FirstOrDefaultAsync();

				var messageDto = new MessageDto
				{
					Id = message.Id,
					Content = message.Content,
					ConversationId = message.ConversationId,
					SenderId = message.SenderId,
					SenderType = message.SenderType,
					SenderName = sender?.FullName ?? "Unknown",
					CreatedAt = message.CreateAt
				};
				await Clients
					.Group(model.ConversationId.ToString())
					.SendAsync("ReceiveMessage", messageDto);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"SendMessage error: {ex}");
				await Clients.Caller.SendAsync("Error", ex.Message);
			}
		}


		public async Task JoinConversation(string conversationId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
		}
	}

}
