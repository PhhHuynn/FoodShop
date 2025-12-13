using ASM.Server.Models;

namespace ASM.Server.DTOs.ConversationDtos
{
	public class ConversationUpdateDto
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public ConversationStatus? Status { get; set; }
	}
}
