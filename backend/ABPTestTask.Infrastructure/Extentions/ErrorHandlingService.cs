using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Extentions
{
    public class ErrorHandlingService
    {
        private readonly ILogger<ErrorHandlingService> _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingService(ILogger<ErrorHandlingService> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        // Middleware method to handle requests
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass the request to the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        // Method to create a response for the exception
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json"; // Set the response content type to JSON
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Set the status code to 500

            // Create a response object with status code, message, and exception details
            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "Internal Server Error. Please try again later.",
                detailed = ex.Message // Include the exception message for debugging purposes
            };

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
