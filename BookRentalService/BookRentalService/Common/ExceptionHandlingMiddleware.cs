using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookRentalService.Common
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, ex.Message);

            // Set the response status code
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Create a response object
            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "An unexpected error occurred. Please try again later.",
                details = ex.Message // You can remove this in production
            };

            // Serialize and write the response
            var jsonResponse = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
