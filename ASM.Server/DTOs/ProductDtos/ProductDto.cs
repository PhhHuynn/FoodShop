namespace ASM.Server.DTOs.ProductDtos
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsAvailable { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public int AverageRating { get; set; }
	}
}
