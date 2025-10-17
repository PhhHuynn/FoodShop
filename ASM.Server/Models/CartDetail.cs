using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Server.Models
{
	public class CartDetail
	{
		public int Id { get; set; }
		public int Quantity { get; set; }

		public int CartId { get; set; }
		public int? FoodId { get; set; }
		public int? ComboId { get; set; }

		[ForeignKey(nameof(FoodId))]
		public Food? Food { get; set; }

		[ForeignKey(nameof(ComboId))]
		public Combo? Combo { get; set; }

		[ForeignKey(nameof(CartId))]
		public Cart Cart { get; set; }

	}
}
