using System.Net;
using Nebulift.Api.Exceptions;
using Nebulift.Api.Models;

namespace Nebulift.Api.Middleware
{
    /// <summary>
    /// Middleware to handle exceptions globally.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next"> An instance of <see cref="RequestDelegate"/>.</param>
        /// <param name="logger">An instance of <see cref="ILogger{ExceptionMiddleware}"/> for logging.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            try
            {
                await _next(httpContext);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError("Not Found: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError("Bad Request: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
                throw; // Rethrow the exception to ensure it is not swallowed
            }
        }

        /// <summary>
        /// Handles the exception and sets the appropriate response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="exception">The exception that was thrown.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            }.ToString());
        }
    }
}