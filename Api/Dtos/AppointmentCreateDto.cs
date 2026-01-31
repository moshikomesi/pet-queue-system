namespace PetQueue.Api.DTOs
{
    public class AppointmentCreateDto
    {
        public int TypeId { get; set; }

        public DateTime ScheduledTime { get; set; }
    }
}