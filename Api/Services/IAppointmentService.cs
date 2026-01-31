using PetQueue.Api.DTOs;
using PetQueue.Api.Models;

namespace PetQueue.Api.Services
{
    public interface IAppointmentService
    {
        // Appointment Creation 
        Task<bool> CreateAsync(int userId, AppointmentCreateDto request);

        //  Retrieval & Filtering 
        // Returns filtered appointments based on client name or specific date
        Task<List<AppointmentResponseDto>> GetAsync();

        Task<List<AppointmentResponseDto>> GetAsync(string? name, DateTime? date);

        // Returns appointments for a specific logged-in user
        Task<List<AppointmentResponseDto>> GetAsync(int userId);

        // Includes security check to ensure the user owns the appointment
        Task<bool> UpdateAsync(int userId, int appointmentId, AppointmentCreateDto request);

        // Includes security check AND prevents deleting same-day appointments
        Task<bool> DeleteAsync(int userId, int appointmentId);

        Task<List<DogType>> GetDogTypesAsync();

    }
}