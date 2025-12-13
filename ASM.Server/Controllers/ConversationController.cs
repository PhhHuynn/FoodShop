using ASM.Server.Data;
using ASM.Server.DTOs.ConversationDtos;
using ASM.Server.DTOs.MessageDtos;
using ASM.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConversationController(AppDbContext context)
        {
            _context = context;
        }

		// GET: api/Conversations
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ConversationDto>>> GetConversations()
		{
			var conversations = await _context.Conversations
				.Include(c => c.Customer)
				.Include(c => c.Messages)
				.ToListAsync();

			var dto = conversations.Select(c => new ConversationDto
			{
				Id = c.Id,
				CustomerId = c.CustomerId,
				CustomerName = c.Customer?.FullName,
				Status = c.Status,
				CreatedAt = c.CreatedAt,
				UpdatedAt = c.UpdatedAt,
				Messages = c.Messages.Select(m => new MessageDto
				{
					Id = m.Id,
					ConversationId = m.ConversationId,
					SenderId = m.SenderId,
					SenderName = m.Sender?.FullName,
					Content = m.Content,
					CreatedAt = m.CreateAt,
					SenderType = m.SenderType
				}).ToList()
			});

			return Ok(dto);
		}


		// GET: api/Conversations/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Conversation>> GetConversation(int id)
        {
            var conversation = await _context.Conversations.FindAsync(id);

            if (conversation == null)
            {
                return NotFound();
            }

            return conversation;
        }

        // PUT: api/Conversations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutConversation(int id, ConversationUpdateDto conversation)
        {
            if (id != conversation.Id)
            {
                return BadRequest();
            }
            var existingConversation = await _context.Conversations.FindAsync(id);
            if (existingConversation == null)
            {
                return NotFound();
			}
            if (conversation.Name != null)
            {
                existingConversation.Name = conversation.Name;
			}
            if (conversation.Status != null)
            {
                existingConversation.Status = conversation.Status.Value;
            }
            existingConversation.UpdatedAt = DateTime.UtcNow;
			try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Conversations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Conversation>> PostConversation(Conversation conversation)
        {
            conversation.CreatedAt = DateTime.UtcNow;
            conversation.UpdatedAt = DateTime.UtcNow;
            conversation.Status = ConversationStatus.Pending;
			_context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConversation", new { id = conversation.Id }, conversation);
        }

		// DELETE: api/Conversations/5
		[HttpDelete("all")]
		public async Task<IActionResult> DeleteAllConversations(){
			var conversations = await _context.Conversations.ToListAsync();
			if (!conversations.Any())
			{
				return NotFound();
			}

			_context.Conversations.RemoveRange(conversations);
			await _context.SaveChangesAsync();

			return NoContent();
		}


		private bool ConversationExists(int id)
        {
            return _context.Conversations.Any(e => e.Id == id);
        }
    }
}
