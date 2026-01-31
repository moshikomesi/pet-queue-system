namespace PetQueue.Api.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        // Relationship
        public List<Appointment> Appointments { get; set; } = new();
    }
}