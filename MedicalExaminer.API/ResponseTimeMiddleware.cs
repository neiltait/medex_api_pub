using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MedicalExaminer.API
{
    /// <summary>
    /// Response Time Middleware.
    /// </summary>
    public class ResponseTimeMiddleware
    {
        /// <summary>
        /// Name of the header
        /// </summary>
        private const string ResponseHeaderResponseTime = "X-Response-Time-ms";

        /// <summary>
        /// Handle to the next Middleware in the pipeline
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialise a new instance of <see cref="ResponseTimeMiddleware"/>.
        /// </summary>
        /// <param name="next">Next delegate.</param>
        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke Async called by MVC.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Task.</returns>
        public Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            context.Response.OnStarting(() =>
            {
                watch.Stop();

                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;

                context.Response.Headers[ResponseHeaderResponseTime] = responseTimeForCompleteRequest.ToString();
                return Task.CompletedTask;
            });

            // Call the next delegate/middleware in the pipeline.
            return _next(context);
        }
    }
}
