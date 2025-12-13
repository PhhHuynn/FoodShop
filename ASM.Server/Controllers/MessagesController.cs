using ASM.Server.Data;
using ASM.Server.DTOs.MessageDtos;
using ASM.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public MessagesController(AppDbContext context)
		{
			_context = context;
		}

		// POST: api/Messages/send
		[HttpPost("send")]
		public async Task<IActionResult> SendMessage([FromBody] MessageCreateDto message)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var exists = await _context.Conversations
				.AnyAsync(c => c.Id == message.ConversationId);
			if (!exists)
				return NotFound(new { error = "Conversation not found." });

			var newMessage = new Message()
			{
				Content = message.Content,
				ConversationId = message.ConversationId,
				SenderId = message.SenderId,
				SenderType = message.SenderType,

			};
			_context.Messages.Add(newMessage);
			await _context.SaveChangesAsync();
			await _context.Conversations.Where(c => c.Id == message.ConversationId)
				.ExecuteUpdateAsync(c => c.SetProperty(c => c.UpdatedAt, DateTime.UtcNow));

			return CreatedAtAction(nameof(GetMessages),
				new { conversationId = message.ConversationId },
				message);
		}

		// GET: api/Messages/conversation/5
		[HttpGet("conversation/{conversationId}")]
		public async Task<IActionResult> GetMessages(int conversationId)
		{
			var messages = await _context.Messages
				.Include(m => m.Sender)
				.Where(m => m.ConversationId == conversationId)
				.OrderBy(m => m.CreateAt)
				.Select(m => new MessageDto
				{
					Id = m.Id,
					Content = m.Content,
					ConversationId = m.ConversationId,
					SenderId = m.SenderId,
					SenderType = m.SenderType,
					SenderName = m.Sender.FullName,
					CreatedAt = m.CreateAt
				})
				.ToListAsync();


			return Ok(messages);
		}
	}
}
