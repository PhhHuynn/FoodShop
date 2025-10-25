using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Server.Models
{
	public class Order
	{
		public int Id { get; set; }

		public string ShippingAddress { get; set; }
		public OrderStatus Status { get; set; }
		public decimal TotalAmount { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public AppUser? User { get; set; }

		public ICollection<OrderDetail>? OrderDetails { get; set; }
	}

	public enum OrderStatus
	{
		Pending = 1, // nhấn đặt hàng nhưng chưa thanh toán hoặc chưa xác nhận
		Confirmed = 2, // đã xác nhận đang làm món
		Delivering = 3, // đang giao hàng
		Completed = 4,
		Cancelled = 5
	}
}
