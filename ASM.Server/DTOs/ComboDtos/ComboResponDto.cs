namespace ASM.Server.Dtos.ComboDtos
{
    public class ComboResponDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
        public bool IsAvailable { get; set; }
		public List<FoodInComboDto>? ComboFoods { get; set; }
        public int AverageRating { get; set; }

	}
}
