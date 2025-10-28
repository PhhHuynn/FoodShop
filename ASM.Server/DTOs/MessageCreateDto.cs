namespace ASM.Server.DTOs
{
	public class MessageCreateDto
	{
		public string Content { get; set; }

		public int ConversationId { get; set; }
		public string SenderId { get; set; }
	}
}
