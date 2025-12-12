using ASM.Server.Models;

namespace ASM.Server.Dtos.OrderDtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string ShippingAddress { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
