using System.Data;
using System.Text;
using Dapper;
using PetQueue.Api.DTOs;
using PetQueue.Api.Models;

namespace PetQueue.Api.Repositories
{
    public class PetQueueRepository : IPetQueueRepository
    {
        private readonly IDbConnection _dbConnection;

        
        public PetQueueRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<AppointmentResponseDto>> GetAsync()
        {
            const string sql = "SELECT * FROM ClientAppointments_View ORDER BY ScheduledTime DESC";
            var result = await _dbConnection.QueryAsync<AppointmentResponseDto>(sql);
            return result.ToList();
        }

        public async Task<List<AppointmentResponseDto>> GetAsync(int userId)
        {
            const string sql = "SELECT * FROM ClientAppointments_View WHERE UserId = @UserId ORDER BY ScheduledTime DESC";
            var result = await _dbConnection.QueryAsync<AppointmentResponseDto>(sql, new { UserId = userId });
            return result.ToList();
        }
        public async Task<bool> CreateAsync(int userId, int typeId, DateTime scheduledTime)
        {
            var parameters = new 
            { 
                UserId = userId, 
                TypeId = typeId, 
                ScheduledTime = scheduledTime 
            };

            //  handles the mapping of anonymous object properties to SP parameters
            var rowsAffected = await _dbConnection.ExecuteAsync(
                "spCreateAppointment", 
                parameters, 
                commandType: CommandType.StoredProcedure);
            
            return rowsAffected > 0;
        }

        public async Task<List<AppointmentResponseDto>> GetAsync(string? clientName, DateTime? date)
        {
            var sql = new StringBuilder("SELECT * FROM ClientAppointments_View WHERE 1=1");
            var parameters = new DynamicParameters();

            if (date.HasValue)
            {
                sql.Append(" AND CAST(ScheduledTime AS DATE) = @Date");
                parameters.Add("Date", date.Value.Date);
            }

            sql.Append(" ORDER BY ScheduledTime DESC");
            var result = await _dbConnection.QueryAsync<AppointmentResponseDto>(sql.ToString(), parameters);
            return result.ToList();
        }

        public async Task<bool> DeleteAsync(int appointmentId)
        {
            // The name of the stored procedure
            const string procedureName = "spDeleteAppointment";
            
            // Execute using CommandType.StoredProcedure
            var rows = await  _dbConnection.QuerySingleAsync<int>(
                procedureName, 
                new { AppointmentId = appointmentId }, 
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        } 

        public async Task<bool> UpdateAsync(int appointmentId, int userId, int typeId, DateTime scheduledTime)
        {
            
            const string procedureName = "spSetAppointment";

            var parameters = new 
            { 
                AppointmentId = appointmentId, 
                UserId = userId, 
                TypeId = typeId, 
                ScheduledTime = scheduledTime 
            };

        var rowsAffected = await _dbConnection.QuerySingleAsync<int>(
            procedureName,
            parameters,
            commandType: CommandType.StoredProcedure
        );

            return rowsAffected > 0;
        }   

        public async Task<List<DogType>> GetDogTypesAsync()
        {
            const string sql = "select * from dbo.DogTypes";
            var result = await _dbConnection.QueryAsync<DogType>(sql);
            return result.ToList();
        }
    }
}