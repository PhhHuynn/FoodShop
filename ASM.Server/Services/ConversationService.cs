using ASM.Server.Data;
using ASM.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM.Server.Services
{
	public  class ConversationService
	{
		private readonly AppDbContext _context;

		public ConversationService(AppDbContext context)
		{
			_context = context;
		}

		public async Task UpdateStatuses()
		{
			
			var now = DateTime.UtcNow;
			await _context.Conversations
				.Where(c => c.Status == ConversationStatus.Open && c.UpdatedAt < now.AddHours(-24))
				.ExecuteUpdateAsync(c => c.SetProperty(c => c.Status, ConversationStatus.Pending)
											.SetProperty(c => c.UpdatedAt, now));

			await _context.Conversations
				.Where(c => c.Status == ConversationStatus.Pending && c.UpdatedAt < now.AddDays(-7))
				.ExecuteUpdateAsync(c => c.SetProperty(c => c.Status, ConversationStatus.Closed)
											.SetProperty(c => c.UpdatedAt, now));

		}
	}
}
