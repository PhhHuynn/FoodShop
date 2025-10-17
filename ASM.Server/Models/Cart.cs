using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Server.Models
{
	public class Cart
	{
		public int Id { get; set; }
		public int Status { get; set; }

		public string UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public AppUser User { get; set; }

		public ICollection<CartDetail> CartDetails { get; set; }
	}

	public enum CartStatus
	{
		Active = 1, // đang hoạt động
		CheckedOut = 2, // đã thanh toán
	}
}
