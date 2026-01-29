using System;

namespace PetQueue.Api.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal FinalPrice { get; set; }

        // Navigation properties 
        public User? User { get; set; }
        public DogType? DogType { get; set; }
    }
}