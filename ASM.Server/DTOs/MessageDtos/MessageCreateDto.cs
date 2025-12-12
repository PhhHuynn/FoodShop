namespace ASM.Server.DTOs.MessageDtos
{
	public class MessageCreateDto
	{
		public int ConversationId { get; set; }
		public string SenderId { get; set; }
		public string Content { get; set; }
	}
}
