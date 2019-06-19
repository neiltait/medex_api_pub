using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MedicalExaminer.API.Controllers;
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
        ///     Called after method executed
        /// </summary>
        /// <remarks>Required by interface. Not intended to be used</remarks>
        /// <param name="context">action context</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        ///     Called before method executes
        /// </summary>
        /// <param name="context">action context</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as BaseController;

            if (controller == null)
            {
                return;
            }

            string controllerName = null;
            string controllerAction = null;

            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                controllerName = controllerActionDescriptor.ControllerName;
                controllerAction = controllerActionDescriptor.ActionName;
            }
            else if (context.ActionDescriptor.RouteValues.Count >= 2)
            {
                controllerName =
                    context.ActionDescriptor.RouteValues.ElementAt(context.ActionDescriptor.RouteValues.Count - 1).Value;
                controllerAction =
                    context.ActionDescriptor.RouteValues.ElementAt(context.ActionDescriptor.RouteValues.Count - 2).Value;
            }

            var logger = controller.Logger;

            var identity = context.HttpContext.User.Identity;
            var userId = ((ClaimsIdentity)identity).Claims.SingleOrDefault(x => x.Type == Authorization.MEClaimTypes.UserId)?.Value;
            var userAuthenticationType = identity.AuthenticationType ?? "Unknown";
            var userIsAuthenticated = identity.IsAuthenticated;
            var parameters = new Dictionary<string, object>();

            foreach (var parameter in context.ActionArguments)
            {
                parameters.Add(parameter.Key, parameter.Value);
            }

            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var remoteIp = remoteIpAddress == null ? "Unknown" : remoteIpAddress.ToString();
            var timeStamp = DateTime.UtcNow;
            logger.Log(
                userId,
                userAuthenticationType,
                userIsAuthenticated,
                controllerName,
                controllerAction,
                parameters,
                remoteIp,
                timeStamp);
        }
    }
}