using Moq;
using PetQueue.Api.Services;
using PetQueue.Api.DTOs;
using PetQueue.Api.Models;
using PetQueue.Api.Repositories;
using Xunit;

namespace PetQueue.Tests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IPetQueueRepository> _repoMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IPetQueueRepository>();
            _service = new AppointmentService(_repoMock.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenAppointmentIsToday()
        {
            // Arrange - today Appointment
            int userId = 1;
            int appId = 10;
            var todayAppointment = new List<AppointmentResponseDto> 
            { 
                new AppointmentResponseDto { 
                    AppointmentId = appId, 
                    UserId = userId, 
                    ScheduledTime = DateTime.Today 
                } 
            };

            _repoMock.Setup(r => r.GetAsync(null, null))
                     .ReturnsAsync(todayAppointment);

            // Act & Assert 
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _service.DeleteAsync(userId, appId));

            Assert.Equal("Appointments scheduled for today cannot be deleted.", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowUnauthorized_WhenUserIsNotOwner()
        {
            // Arrange - update Appointment by another user
            int realOwnerId = 99;
            int hackerId = 1;
            int appId = 10;
            
            var appointment = new List<AppointmentResponseDto> 
            { 
                new AppointmentResponseDto { AppointmentId = appId, UserId = realOwnerId, ScheduledTime = DateTime.Today.AddDays(1) } 
            };

            _repoMock.Setup(r => r.GetAsync(null, null))
                     .ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => 
                _service.DeleteAsync(hackerId, appId));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenDateIsInPast()
        {
            // Arrange
            var pastDate = DateTime.Now.AddDays(-1);
            var request = new AppointmentCreateDto { TypeId = 1, ScheduledTime = pastDate };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _service.CreateAsync(userId: 1, request));

            Assert.Equal("Appointments must be scheduled for a future date.", exception.Message);
        }
    }
}