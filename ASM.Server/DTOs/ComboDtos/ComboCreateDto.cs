namespace ASM.Server.Dtos.ComboDtos
{
    public class ComboCreateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public IFormFile? FImageFile { get; set; }
        public List<ComboFoodCreateDto> ComboFoods { get; set; }
    }
}
