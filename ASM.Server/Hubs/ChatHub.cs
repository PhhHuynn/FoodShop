using Microsoft.AspNetCore.SignalR;

namespace ASM.Server.Hubs
{
	public class ChatHub : Hub
	{
		public async Task SendMessage(string senderId, string receiverId, string message)
		{
			await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
		}
	}
}
