namespace ASM.Server.Dtos.ComboDtos
{
    public class ComboUpdateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile? FImageFile { get; set; }
        public List<ComboFoodUpdateDto> ComboFoods { get; set; }
    }
}
