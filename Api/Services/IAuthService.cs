using PetQueue.Api.DTOs;

namespace PetQueue.Api.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegisterDto request);
        Task<string> LoginAsync(UserLoginDto request);
    }
}