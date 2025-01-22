using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Nebulift.Api.Exceptions;
using Nebulift.Api.Models;

namespace Nebulift.Api.Middleware
{
    /// <summary>
    /// Middleware to handle exceptions globally.
    /// Implements <see cref="IMiddleware"/> and <see cref="IExceptionHandler"/>.
    /// </summary>
    public class ExceptionMiddleware : IMiddleware, IExceptionHandler
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger{ExceptionMiddleware}"/> for logging.</param>
        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Main middleware to handle exceptions.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError("Not Found: {Message}", ex.Message);
                await TryHandleAsync(context, ex, CancellationToken.None);
            }
            catch (FailedTemplateExecutionException ex)
            {
                _logger.LogError("Bad Request: {Message}", ex.Message);
                await TryHandleAsync(context, ex, CancellationToken.None);
            }
            catch (MissingInputsException ex)
            {
                _logger.LogError("Missing inputs: {Message}", ex.Message);
                await TryHandleAsync(context, ex, CancellationToken.None);
            }
            catch (NotImplementedException ex)
            {
                _logger.LogError("Not implemented: {Message}", ex.Message);
                await TryHandleAsync(context, ex, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred: {Message}", ex.Message);
                await TryHandleAsync(context, ex, CancellationToken.None);
                throw; // Rethrow the exception to ensure it is not swallowed
            }
        }

        /// <summary>
        /// Attempts to handle the specified exception asynchronously.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the exception was handled; otherwise, false.</returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (httpContext == null || exception == null)
            {
                return false;
            }

            httpContext.Response.ContentType = "application/json";

            // Determine the status code based on the exception
            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                FailedTemplateExecutionException => (int)HttpStatusCode.InternalServerError,
                MissingInputsException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            // Log the exception
            _logger.LogError("Exception handled by IExceptionHandler: {Message}", exception.Message);

            // Write the response as JSON
            var errorDetails = new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exception.Message,
            };

            await httpContext.Response.WriteAsync(errorDetails.ToString(), cancellationToken);
            return true; // Indicates the exception was handled
        }
    }
}
