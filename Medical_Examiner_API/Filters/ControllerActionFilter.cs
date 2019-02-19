using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;


namespace Medical_Examiner_API
{
    public class ControllerActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Medical_Examiner_API.Controllers.BaseController;

            if (controller == null)
                return;

            var logger = controller.Logger;

            var identity = context.HttpContext.User.Identity;
            var userName = identity.Name ?? "Unknown";
            var userAuthenticationType = identity.AuthenticationType ?? "Unknown";
            var userIsAuthenticated = identity.IsAuthenticated;
            var routeDataValues = context.RouteData.Values.Values;
            var controllerName = routeDataValues.Count >= 2 ? routeDataValues.ElementAt(1).ToString(): "Unknown";
            var parameters = new List<string>();

            foreach (var parameter in context.ActionArguments)
            {
                var paramterItem = $"{parameter.Key}: {parameter.Value}";
                parameters.Add(paramterItem);
                
            }
            var controllerAction = routeDataValues.Count >= 1 ? routeDataValues.ElementAt(0).ToString() : "Unknown";
            var remoteIPAddress = context.HttpContext.Connection.RemoteIpAddress;
            var remoteIP = remoteIPAddress == null ? "Unknown" : remoteIPAddress.ToString();
            var timeStamp = DateTime.UtcNow;

            logger.Log(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerAction, parameters, remoteIP, timeStamp);
 
        }
    }
}
