using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.API
{
    /// <summary>
    /// Global Exception Filter.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        /// <summary>
        /// Initialise a new instance of <see cref="GlobalExceptionFilter"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public void OnException(ExceptionContext context)
        {
            _logger.LogCritical(context.Exception, "A global exception has been caught");

            context.Result = new JsonResult(new GlobalError("An unhandled exception occured and was logged."));

            // Remove cache header
            context.HttpContext.Response.Headers.Remove("cache-control");

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        private class GlobalError
        {
            public GlobalError(string message)
            {
                Message = message;
            }

            // ReSharper disable once MemberCanBePrivate.Local
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Message { get; }
        }
    }
}
