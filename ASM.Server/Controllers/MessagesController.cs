using ASM.Server.Data;
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
		public async Task<IActionResult> SendMessage([FromBody] Message message)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var exists = await _context.Conversations
				.AnyAsync(c => c.Id == message.ConversationId);
			if (!exists)
				return NotFound(new { error = "Conversation not found." });

			_context.Messages.Add(message);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetMessages),
				new { conversationId = message.ConversationId },
				message);
		}

		// GET: api/Messages/conversation/5
		[HttpGet("conversation/{conversationId}")]
		public async Task<IActionResult> GetMessages(int conversationId)
		{
			var messages = await _context.Messages
				.Where(m => m.ConversationId == conversationId)
				.Include(m => m.Sender) 
				.OrderBy(m => m.CreateAt)
				.ToListAsync();

			if (!messages.Any())
				return NotFound(new { message = "No messages found for this conversation." });

			return Ok(messages);
		}
	}
}
