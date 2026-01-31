using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PetQueue.Api.DTOs;
using PetQueue.Api.Services;
using System.Security.Claims;

namespace PetQueue.Api.Endpoints
{
    public static class AppointmentEndpoints
    {
        public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app)
        {
            // Public endpoints (no auth required)
            
            var publicGroup = app.MapGroup("/api/appointments")
                .WithTags("Appointments - Public");

            // GET /api/appointments/types (No Auth Required)

            publicGroup.MapGet("/types", async (IAppointmentService service) =>
            {
                try
                {
                    var types = await service.GetDogTypesAsync();
                    return Results.Ok(types);
                }
                catch (Exception)
                {
                    return Results.Problem("Failed to retrieve dog types.");
                }
            })
            .WithName("GetDogTypes")
            .WithDescription("Get all available dog types")
            .WithOpenApi();

            // Protected endpoints (auth required)
            var group = app.MapGroup("/api/appointments")
                .RequireAuthorization()
                .WithTags("Appointments - Protected");

            // GET /api/appointments

            group.MapGet("/", async (
                ClaimsPrincipal user,
                IAppointmentService service) =>
            {
                try
                {
                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim is null)
                        return Results.Unauthorized();

                    var userId = int.Parse(userIdClaim.Value);
                    var appointments = await service.GetAsync();
                    return Results.Ok(appointments);
                }
                catch (Exception)
                {
                    return Results.Problem("Failed to retrieve appointments.");
                }
            })
            .WithName("GetAppointments")
            .WithDescription("Get all appointments for the current user")
            .WithOpenApi();

            // GET /api/appointments/{id}

            group.MapGet("/{id:int}", async (
                int id,
                ClaimsPrincipal user,
                IAppointmentService service) =>
            {
                try
                {
                    //var userIdClaim = user.FindFirst(JwtRegisteredClaimNames.Sub);
                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim is null)
                        return Results.Unauthorized();

                    var userId = int.Parse(userIdClaim.Value);
                    
                    var appointments = await service.GetAsync(userId);
                    var appointment = appointments.FirstOrDefault(a => a.AppointmentId == id);
                    
                    return appointment is not null
                        ? Results.Ok(appointment)
                        : Results.NotFound(new { error = "Appointment not found." });
                }
                catch (Exception)
                {
                    return Results.Problem("Failed to retrieve appointment.");
                }
            })
            .WithName("GetAppointmentById")
            .WithDescription("Get a specific appointment by ID")
            .WithOpenApi();

            // POST /api/appointments
            
            group.MapPost("/", async (
                AppointmentCreateDto request,
                ClaimsPrincipal user,
                IAppointmentService service) =>
            {
                try
                {
                    if (!user.Identity?.IsAuthenticated ?? true)
                        return Results.Unauthorized();

                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                    if (userIdClaim is null)
                        return Results.Unauthorized();

                    var userId = int.Parse(userIdClaim.Value);

                    var success = await service.CreateAsync(userId, request);

                    return success
                        ? Results.Created($"/api/appointments/{userId}", new { message = "Appointment created successfully." })
                        : Results.BadRequest(new { error = "Failed to create appointment." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return Results.Problem(detail: ex.Message);
                }
            })
            .WithName("CreateAppointment")
            .WithDescription("Create a new appointment")
            .WithOpenApi();

            
            // PUT /api/appointments/{id}
           
            group.MapPut("/{id:int}", async (
                int id,
                AppointmentCreateDto request,
                HttpContext httpContext,
                IAppointmentService service) =>
            {
                try
                {
                    var userIdClaim =
                        httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                    if (userIdClaim == null)
                        return Results.Unauthorized();

                    var userId = int.Parse(userIdClaim.Value);
                    
                    var success = await service.UpdateAsync(userId, id, request);
                    return success 
                        ? Results.Ok(new { message = "Appointment updated successfully." })
                        : Results.NotFound(new { error = "Appointment not found." ,success});
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Forbid();
                }
                catch (Exception)
                {
                    return Results.Problem("An error occurred while updating the appointment.");
                }
            })
            .WithName("UpdateAppointment")
            .WithDescription("Update an existing appointment")
            .WithOpenApi();

            
            // DELETE /api/appointments/{id}
           
            group.MapDelete("/{id:int}", async (
                int id,
                HttpContext httpContext,
                IAppointmentService service) =>
            {
                try
                {
                    var userIdClaim =
                        httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                    if (userIdClaim == null)
                        return Results.Unauthorized();

                    var userId = int.Parse(userIdClaim.Value);

                    var success = await service.DeleteAsync(userId, id);
                    return success 
                        ? Results.NoContent()
                        : Results.NotFound(new { error = "Appointment not found." });
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Forbid();
                }
                catch (Exception)
                {
                    return Results.Problem("An error occurred while deleting the appointment.");
                }
            })
            .WithName("DeleteAppointment")
            .WithDescription("Delete an appointment")
            .WithOpenApi();
        }
    }
}
