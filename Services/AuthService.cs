using Microsoft.EntityFrameworkCore;
using PetQueue.Api.Data;
using PetQueue.Api.Models;
using PetQueue.Api.DTOs;
using PetQueue.Api.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace PetQueue.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository; 
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(UserRegisterDto request)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidOperationException("Username and password are required.");

            // Check if user already exists to avoid duplicates
            var existingUser = await _repository.GetAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("User already exists.");

            var user = new User
            {
                Username = request.Username,
                FirstName = request.FirstName,
                // Automatically salts and hashes the password
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            // Save the user via repository
            await _repository.CreateAsync(user);
            
            return CreateToken(user.UserId, user.Username);
        }

        public async Task<string> LoginAsync(UserLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidOperationException("Username and password are required.");

            var user = await _repository.GetAsync(request.Username);
            
            if (user == null)
                throw new InvalidOperationException("Invalid username or password.");

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValid)
                throw new InvalidOperationException("Invalid username or password.");

            return CreateToken(user.UserId, user.Username);
        }

        public string CreateToken(int userId, string username)
        {
            var jwt = _configuration.GetSection("Jwt");

            var claims = new[]
            {
              //  new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"])
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwt["ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}