using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Reporting;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MedicalExaminer.API.Filters
{
    /// <summary>
    ///     Handles pre and post action handling
    /// </summary>
    public class ControllerActionFilter : IActionFilter
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> _logger;

        private readonly RequestChargeService _requestChargeService;

        private DateTime _timestamp;

        private Dictionary<string, object> _parameters;

        /// <summary>
        /// Initialise a new instance of <see cref="ControllerActionFilter"/>.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ControllerActionFilter(IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger, RequestChargeService requestChargeService)
        {
            _logger = logger;
            _requestChargeService = requestChargeService;
        }

        /// <summary>
        ///     Called after method executed
        /// </summary>
        /// <remarks>Required by interface. Not intended to be used</remarks>
        /// <param name="context">action context</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            string controllerName = null;
            string controllerAction = null;

            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                controllerName = controllerActionDescriptor.ControllerName;
                controllerAction = controllerActionDescriptor.ActionName;
            }

            var identity = context.HttpContext.User.Identity;
            var userId = ((ClaimsIdentity)identity).Claims.SingleOrDefault(x => x.Type == Authorization.MEClaimTypes.UserId)?.Value;
            var userAuthenticationType = identity.AuthenticationType ?? "Unknown";
            var userIsAuthenticated = identity.IsAuthenticated;

            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var remoteIp = remoteIpAddress == null ? "Unknown" : remoteIpAddress.ToString();
            var totalRus = _requestChargeService.RequestCharges.Sum(i => i.Charge);

            _logger.Handle(new CreateMELoggerQuery(new LogMessageActionDefault(
                userId,
                userAuthenticationType,
                userIsAuthenticated,
                controllerName,
                controllerAction,
                _parameters,
                remoteIp,
                _timestamp,
                totalRus
            )));
        }

        /// <summary>
        ///     Called before method executes
        /// </summary>
        /// <param name="context">action context</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _timestamp = DateTime.UtcNow;
            _parameters = new Dictionary<string, object>();

            foreach (var parameter in context.ActionArguments)
            {
                _parameters.Add(parameter.Key, parameter.Value);
            }
        }
    }
}