using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASM.Server.Models
{
	public class OrderDetail
	{
		public int Id { get; set; }

		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public int OrderId { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(OrderId))]
		public Order? Order { get; set; }

		public int? FoodId { get; set; }
		public int? ComboId { get; set; }

		[ForeignKey(nameof(FoodId))]
		public Food? Food { get; set; }

		[ForeignKey(nameof(ComboId))]
		public Combo? Combo { get; set; }

	}
}
