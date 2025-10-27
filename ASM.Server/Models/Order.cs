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
		Pending = 1, // làm món
		Shipping = 2, // đang giao hàng
		Delivered = 3, // đã giao
	}
}
