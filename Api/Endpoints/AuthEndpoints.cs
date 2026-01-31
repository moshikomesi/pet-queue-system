using PetQueue.Api.DTOs;
using PetQueue.Api.Services;

namespace PetQueue.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth")
                .WithTags("Auth")
                .WithOpenApi();

            group.MapPost("/register", async (UserRegisterDto request, IAuthService authService) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                        return Results.BadRequest(new { error = "Username and password are required." });

                    var token = await authService.RegisterAsync(request);
                    
                    if (string.IsNullOrWhiteSpace(token))
                        return Results.BadRequest(new { error = "Failed to generate authentication token." });

                    return Results.Ok(new { token, message = "Registration successful." });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception)
                {
                    return Results.Problem("An error occurred during registration.");
                }
            })
            .WithName("Register")
            .WithDescription("Register a new user account");

            group.MapPost("/login", async (UserLoginDto request, IAuthService authService) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                        return Results.BadRequest(new { error = "Username and password are required." });

                    var token = await authService.LoginAsync(request);

                    if (string.IsNullOrWhiteSpace(token))
                        return Results.Unauthorized();

                    return Results.Ok(new { token, message = "Login successful." });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Unauthorized();
                }
                catch (Exception)
                {
                    return Results.Problem("An error occurred during login.");
                }
            })
            .WithName("Login")
            .WithDescription("Login with username and password");
        }
    }
}