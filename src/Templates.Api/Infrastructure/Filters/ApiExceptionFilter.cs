using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Templates.Api.Infrastructure.Responses;

namespace Templates.Api.Infrastructure.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

            var response = exception switch
            {
                ArgumentOutOfRangeException => ApiResponse<string>.Fail(exception.Message),
                KeyNotFoundException => ApiResponse<string>.Fail(exception.Message),
                InvalidOperationException => ApiResponse<string>.Fail(exception.Message),
                _ => ApiResponse<string>.Fail("An unexpected error occurred")
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = exception switch
                {
                    ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    InvalidOperationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                }
            };

            context.ExceptionHandled = true;
        }
    }
}
