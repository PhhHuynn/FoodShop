using ASM.Server.Models;

namespace ASM.Server.Dtos.OrderDtos
{
    public class OrderCreateDto
    {
        public int Id { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int? VoucherId { get; set; }

        public List<OrderDetailDto>? OrderDetails { get; set; }
    }
}
