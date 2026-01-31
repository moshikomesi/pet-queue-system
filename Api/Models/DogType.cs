namespace PetQueue.Api.Models
{
    public class DogType
    {
            public int TypeId { get; set; }
            public string TypeName { get; set; } = string.Empty; 
            public int Duration { get; set; }
            public decimal Price { get; set; }
    }
}