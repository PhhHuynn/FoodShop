using System.ComponentModel.DataAnnotations;

namespace ASM.Server.Models
{
	public class Combo : Product
	{
        public ICollection<ComboFood>? ComboFoods { get; set; }
	}
}
