using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Server.Models
{
	public class Conversation
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public ConversationStatus Status { get; set; } = ConversationStatus.Open;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }

		public string CustomerId { get; set; }

		[ForeignKey(nameof(CustomerId))]
		public AppUser? Customer { get; set; }

		public ICollection<Message>? Messages { get; set; }
	}

	public enum ConversationStatus
	{
		Open = 1,        // cuộc trò chuyện đang hoạt động
		Closed = 2,        // đã kết thúc
		Pending = 3,       // đang chờ nhân viên phản hồi
		Archived = 4       // lưu trữ, không hiển thị nữa
	}
}
