using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Server.Models
{
	public class Food : ProductBase
	{
		public int CategoryId { get; set; }
		[ForeignKey(nameof(CategoryId))]
		public Category? Category { get; set; }
		public ICollection<ComboFood> ComboFoods { get; set; }
	}
}
