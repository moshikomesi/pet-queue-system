using System.Net;
using System.Text.Json;

namespace PetQueue.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                
                // קביעת קוד הסטטוס לפי סוג השגיאה
                context.Response.StatusCode = ex switch
                {
                    UnauthorizedAccessException => (int)HttpStatusCode.Forbidden,
                    InvalidOperationException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                    Detail = "Check if table names in SQL match your Repository code." 
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}