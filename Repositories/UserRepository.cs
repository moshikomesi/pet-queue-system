using System.Data;
using System.Text;
using Dapper;
using PetQueue.Api.DTOs;
using PetQueue.Api.Models;

namespace PetQueue.Api.Repositories
{
   public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

public async Task<User?> GetAsync(string username)
{
    const string procedureName = "spGetUserByUsername";
    
    return await _dbConnection.QueryFirstOrDefaultAsync<User>(
        procedureName, 
        new { Username = username }, 
        commandType: CommandType.StoredProcedure
    );
}

public async Task<bool> CreateAsync(User user)
{
    const string procedureName = "spCreateUser";

    // We pass the object directly, Dapper matches properties to @parameters
    var result = await _dbConnection.ExecuteAsync(
        procedureName, 
        new { 
            user.Username, 
            user.PasswordHash, 
            user.FirstName 
        }, 
        commandType: CommandType.StoredProcedure
    );

    return result > 0;
}
}
}