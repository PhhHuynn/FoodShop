using System.ComponentModel.DataAnnotations;

namespace ASM.Server.Models
{
	public class Combo : ProductBase
	{
		public ICollection<ComboFood>? ComboFoods { get; set; }
	}
}
