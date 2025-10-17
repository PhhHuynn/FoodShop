using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Server.Models
{
	public class Message
	{
		public int Id { get; set; }

		public string Content { get; set; }
		public bool IsRead { get; set; }
		public DateTime CreateAt { get; set; }

		public int ConversationId { get; set; }
		public string SenderId { get; set; }

		[ForeignKey(nameof(ConversationId))]
		public Conversation Conversation { get; set; }

		[ForeignKey(nameof(SenderId))]
		public AppUser Sender { get; set; }
	}
}
