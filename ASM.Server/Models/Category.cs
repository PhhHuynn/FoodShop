using System.ComponentModel.DataAnnotations;

namespace ASM.Server.Models
{
	public class Category
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public ICollection<Food>? Foods { get; set; }
	}
}
