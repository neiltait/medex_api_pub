using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Medical_Examiner_API;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;

namespace Medical_Examiner_API_Tests.ControllerTests
{
    public class ControllerActionFilterTests
    {
        private MELoggerMocker _mockLogger;
        private ValuesController _controller;

        public ControllerActionFilterTests()
        {
            _mockLogger = new MELoggerMocker();
            _controller = new ValuesController(_mockLogger);
        }

        [Fact]
        public void CheckCallToLogger()
        {
            var controllerActionFilter = new ControllerActionFilter();
            var actionContext = new ActionContext();
            actionContext.HttpContext = new MockHttpContext();
            var identity = new ClaimsIdentity();
            actionContext.HttpContext.User.AddIdentity(identity);
            actionContext.RouteData = new Microsoft.AspNetCore.Routing.RouteData();
            actionContext.RouteData.Values.Add("Action", "MyAction");
            actionContext.RouteData.Values.Add("Method", "MyMethod");
            actionContext.ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();
            var filters = new List<IFilterMetadata>();
            var actionArguments = new Dictionary<string, object>();
            var actionExecutingContext = new ActionExecutingContext(actionContext, filters, actionArguments, _controller);
            controllerActionFilter.OnActionExecuting(actionExecutingContext);
            var logEntry = _mockLogger.LogEntry;

            var logEntryContents = logEntry.UserName + " " + logEntry.UserAuthenticationType + " " + logEntry.UserIsAuthenticated.ToString() + " " + logEntry.ControllerName + " " + logEntry.ControllerMethod + " " + logEntry.RemoteIP;

            var expectedMessage = "Unknown Unknown False MyMethod MyAction Unknown";
            Assert.Equal(expectedMessage, logEntryContents);
        }
    }
}
