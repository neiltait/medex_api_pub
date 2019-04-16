using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.Log(LogLevel.Critical, "A global exception has been caught: ", context.Exception);

            //Remove cache header
            context.HttpContext.Response.Headers.Remove("cache-control");

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        public void Dispose()
        { }
    }
}
