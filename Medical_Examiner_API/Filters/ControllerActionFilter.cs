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
            var toDo = 1;
         
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Medical_Examiner_API.Controllers.BaseController;
            var logger = controller.Logger;

            var userName = "User: " + (context.HttpContext.User.Identity.Name == null? "Unknown": context.HttpContext.User.Identity.Name);
            var userAuthenticationType = " AuthenticationType: " + (context.HttpContext.User.Identity.AuthenticationType == null ? "Unknown" : context.HttpContext.User.Identity.AuthenticationType);
            var userIsAuthenticated = " IsAuthenticated: " + (context.HttpContext.User.Identity.IsAuthenticated.ToString());
            var controllerName = context.RouteData.Values.Values.ElementAt(1).ToString();
            var parameters = new List<string>();

            foreach (var parameter in context.ActionArguments)
            {
                var paramterItem = parameter.Key.ToString() + ": " + parameter.Value == null ? "" : parameter.Value.ToString();
                parameters.Add(paramterItem);
                
            }
            var controllerAction = context.RouteData.Values.Values.ElementAt(0).ToString();
            var remoteIP = context.HttpContext.Connection.RemoteIpAddress == null ? "Unknown" : context.HttpContext.Connection.RemoteIpAddress.ToString();
            var timeStamp = DateTime.UtcNow;

            logger.Log(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerAction, parameters, remoteIP, timeStamp);


            var toDo = 1;
         
            
        }
    }
}
