using ASM.Server.Models;

namespace ASM.Server.DTOs.ConversationDtos
{
	public class ConversationDto
	{
		public int Id { get; set; }
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public ConversationStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public List<MessageDtos.MessageDto> Messages { get; set; }
	}
}
