namespace ASM.Server.DTOs.MessageDtos
{
	public class MessageDto
	{
		public int Id { get; set; }
		public int ConversationId { get; set; }
		public string SenderId { get; set; }
		public string SenderName { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? SenderType { get; set; }
	}
}
