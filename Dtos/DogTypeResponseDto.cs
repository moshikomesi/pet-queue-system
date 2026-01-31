namespace PetQueue.Api.DTOs
{
    public class DogTypeResponseDto
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal BasePrice { get; set; }
    }
}