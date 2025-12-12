using ASM.Server.Dtos.Cart;

namespace ASM.Server.Dtos.CartDtos
{
    public class CartResponseDto
    {
		public List<CartDetailResponseDto> CartDetails { get; set; } = new();
		public decimal Total { get; set; }
	}
}
