using System;
using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.API.Controllers;
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

            var logger = controller.Logger;

            var identity = context.HttpContext.User.Identity;
            var userName = identity.Name ?? "Unknown";
            var userAuthenticationType = identity.AuthenticationType ?? "Unknown";
            var userIsAuthenticated = identity.IsAuthenticated;
            var routeDataValues = context.RouteData.Values.Values;
            var controllerName = routeDataValues.Count >= 2 ? routeDataValues.ElementAt(1).ToString() : "Unknown";
            var parameters = new List<string>();

            foreach (var parameter in context.ActionArguments)
            {
                var paramterItem = $"{parameter.Key}: {parameter.Value}";
                parameters.Add(paramterItem);
            }

            var controllerAction = routeDataValues.Count >= 1 ? routeDataValues.ElementAt(0).ToString() : "Unknown";
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var remoteIp = remoteIpAddress == null ? "Unknown" : remoteIpAddress.ToString();
            var timeStamp = DateTime.UtcNow;
            logger.Log(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerAction,
                parameters, remoteIp, timeStamp);
        }
    }
}