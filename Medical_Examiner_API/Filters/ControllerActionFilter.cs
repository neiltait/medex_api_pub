using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


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
            var controllerLogger = (context.Controller as Medical_Examiner_API.Controllers.BaseController).Logger;

            var userName = "User: " + (context.HttpContext.User.Identity.Name == null? "Unknown": context.HttpContext.User.Identity.Name);
            var userAuthenticationType = " AuthenticationType: " + (context.HttpContext.User.Identity.AuthenticationType == null ? "Unknown" : context.HttpContext.User.Identity.AuthenticationType);
            var userIsAuthenticated = " IsAuthenticated: " + (context.HttpContext.User.Identity.IsAuthenticated.ToString());
            var controllerName = context.RouteData.Values.Values.ElementAt(1);
            var parameters = new List<string>();

            foreach (var parameter in context.ActionArguments)
            {
                var paramterItem = parameter.Key.ToString() + ": " + parameter.Value == null ? "" : parameter.Value.ToString();
                parameters.Add(paramterItem);
                
            }
            var controllerAction = context.RouteData.Values.Values.ElementAt(0);
            var remoteIP = context.HttpContext.Connection.RemoteIpAddress == null ? "Unknown" : context.HttpContext.Connection.RemoteIpAddress.ToString();
            var timeStamp = DateTime.UtcNow;

            var message = userName + userAuthenticationType + userIsAuthenticated;
            controllerLogger.LogInformation(message);
 


            var toDo = 1;
            //var djpFile = new StreamWriter(@"C:\\Temp\djp.txt");
            //djpFile.WriteLine("OnActionExecuting called");
            
        }
    }
}
