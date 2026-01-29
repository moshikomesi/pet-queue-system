using PetQueue.Api.Models;

namespace PetQueue.Api.Repositories
{
 public interface IUserRepository
{
    Task<User?> GetAsync(string username);
    Task<bool> CreateAsync(User user);
}

}