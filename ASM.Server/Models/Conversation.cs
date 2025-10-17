using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Server.Models
{
	public class Conversation
	{
		public int Id { get; set; }

		public string? Name { get; set; }
		public string? Status { get; set; }
		public DateTime CreatedAt { get; set; }

		public string CustomerId { get; set; }
		public string EmployeeId { get; set; }

		[ForeignKey(nameof(CustomerId))]
		public AppUser Customer { get; set; }

		[ForeignKey(nameof(EmployeeId))]
		public AppUser Employee { get; set; }

		public ICollection<Message> Messages { get; set; }
	}
}
