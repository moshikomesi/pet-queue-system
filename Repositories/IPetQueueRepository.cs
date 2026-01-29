using PetQueue.Api.DTOs;
using PetQueue.Api.Models;

namespace PetQueue.Api.Repositories
{
    public interface IPetQueueRepository
    {
        // Executes the stored procedure with loyalty logic
        Task<bool> CreateAsync(int userId, int typeId, DateTime scheduledTime);
        Task<bool> UpdateAsync(int appointmentId, int userId, int typeId, DateTime scheduledTime);
        Task<bool> DeleteAsync(int appointmentId);
        Task<List<DogType>> GetDogTypesAsync();
        
        // Fetches directly from the SQL View into the Response DTO
        Task<List<AppointmentResponseDto>> GetAsync();
        Task<List<AppointmentResponseDto>> GetAsync(int userId);
        Task<List<AppointmentResponseDto>> GetAsync(string? clientName, DateTime? date);
    }
}