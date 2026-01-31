namespace PetQueue.Api.DTOs
{
    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string DogSize { get; set; } = string.Empty;
        public DateTime ScheduledTime { get; set; }
        public decimal FinalPrice { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}