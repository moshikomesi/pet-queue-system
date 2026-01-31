using Moq;
using PetQueue.Api.Services;
using PetQueue.Api.Models;
using PetQueue.Api.DTOs;
using PetQueue.Api.Repositories;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace PetQueue.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();

            _configMock.Setup(x => x["AppSettings:Token"]).Returns("super_secret_test_key_1234567890");

            _authService = new AuthService(_repoMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
        {
            var loginRequest = new UserLoginDto { Username = "ghost_user", Password = "123" };
            
            // Fixed method name here:
            _repoMock.Setup(r => r.GetAsync(loginRequest.Username))
                     .ReturnsAsync((User)null);

            var result = await _authService.LoginAsync(loginRequest);

            Assert.Null(result);
        }
        
      
    }
}