using Microsoft.AspNetCore.SignalR;

namespace ASM.Server.Hubs
{
	public class ChatHub : Hub
	{
		public async Task SendMessage(string conversationId, string senderId, string message)
		{
			await Clients.Group(conversationId).SendAsync("ReceiveMessage", senderId, message);
		}

		public async Task JoinConversation(string conversationId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
		}
	}

}
