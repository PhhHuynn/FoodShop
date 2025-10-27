using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASM.Server.Models
{
	public abstract class ProductBase
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsAvailable { get; set; } = true;

		[JsonIgnore]
		public ICollection<OrderDetail>? OrderDetails { get; set; }
	}
}
