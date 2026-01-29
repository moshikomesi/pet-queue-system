using System.Text;
using PetQueue.Api.DTOs;
using PetQueue.Api.Models;
using PetQueue.Api.Repositories;
namespace PetQueue.Api.Services
{
    public class AppointmentService : IAppointmentService
{
    private readonly IPetQueueRepository _repository;

    public AppointmentService(IPetQueueRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateAsync(int userId, AppointmentCreateDto request)
    {
    // 1. Basic Validation: Ensure the appointment is for a future date
    if (request.ScheduledTime <= DateTime.UtcNow)
    {
        throw new InvalidOperationException("Appointments must be scheduled for a future date.");
    }

    return await _repository.CreateAsync(
        userId, 
        request.TypeId, 
        request.ScheduledTime);
    }
    public async Task<List<AppointmentResponseDto>> GetAsync()
    {
        var appointments = await _repository.GetAsync();
        return appointments.OrderBy(a => a.ScheduledTime).ToList();
    }
    public async Task<List<AppointmentResponseDto>> GetAsync(int userId)
    {
        var appointments = await _repository.GetAsync(userId);
        return appointments.OrderBy(a => a.ScheduledTime).ToList();
    }
    public async Task<List<AppointmentResponseDto>> GetAsync(string? name, DateTime? date)
    {
        var appointments = await _repository.GetAsync(name, date);
        return appointments.OrderBy(a => a.ScheduledTime).ToList();
    }

    public async Task<bool> DeleteAsync(int userId, int appointmentId)
    {
        // Fetch specifically by ID - ensure the repo returns a single object or a list
        var appointments = await _repository.GetAsync(null, null);
        var appointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

        if (appointment == null) return false;

        // Security check: Use UserId from the DTO
        if (appointment.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to delete this appointment.");

        // Business rule check
        if (appointment.ScheduledTime.Date == DateTime.UtcNow.Date)
            throw new InvalidOperationException("Appointments scheduled for today cannot be deleted.");

        return await _repository.DeleteAsync(appointmentId);
    }

    public async Task<bool> UpdateAsync(int userId, int appointmentId, AppointmentCreateDto request)
    {
        // Fetch all appointments (filtered by the repository logic)
        var appointments = await _repository.GetAsync(null, null);
        
        // Find the specific appointment from the list
        var appointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

        // 1. Check if appointment exists
        if (appointment == null) 
        {
            return false;
        }

        // 2. Requirement: Client cannot edit records that don't belong to them
        if (appointment.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to edit this appointment.");
        }
        
        // 3. Optional: Add a check if the appointment is in the past
        if (appointment.ScheduledTime < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Cannot edit a past appointment.");
        }

        // Delegate the update to the repository
        return await _repository.UpdateAsync(appointmentId, userId, request.TypeId, request.ScheduledTime);
    }
    
    public async Task<List<DogType>> GetDogTypesAsync()
        {
            return await _repository.GetDogTypesAsync();
        }
}
}