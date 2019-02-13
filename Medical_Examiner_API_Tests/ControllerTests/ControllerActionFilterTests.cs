
using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using System.Threading;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Medical_Examiner_API_Tests.ControllerTests
{
    class MockConnectionInfo : ConnectionInfo
    {
        public override string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IPAddress RemoteIpAddress { get { return null; } set { } }
        public override int RemotePort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IPAddress LocalIpAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int LocalPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override X509Certificate2 ClientCertificate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override Task<X509Certificate2> GetClientCertificateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    class MockHttpContext : HttpContext
    {
        private ClaimsPrincipal _claimsPrincipal;
        private ConnectionInfo _connectionInfo;

        public MockHttpContext() :base()
        {
            _claimsPrincipal = new ClaimsPrincipal();
            _connectionInfo = new MockConnectionInfo();
        }

        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest Request => throw new NotImplementedException();

        public override HttpResponse Response => throw new NotImplementedException();

        public override ConnectionInfo Connection { get { return _connectionInfo; } }

        public override WebSocketManager WebSockets => throw new NotImplementedException();

     
         
        public override ClaimsPrincipal User { get { return _claimsPrincipal; } set { _claimsPrincipal = value; } }
        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Microsoft.AspNetCore.Http.Authentication.AuthenticationManager Authentication => throw new NotImplementedException();

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
    public class ControllerActionFilterTests
    {
        MELoggerMocker _mockLogger;
        DJPTestController _controller;

        public ControllerActionFilterTests()
        {
            _mockLogger = new MELoggerMocker();
            _controller = new DJPTestController(_mockLogger); 
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
            var actionExecutingContext = new ActionExecutingContext(actionContext, filters, actionArguments, _controller );
            controllerActionFilter.OnActionExecuting(actionExecutingContext);
            var message = _mockLogger.Message;

            var expectedMessage = "Unknown Unknown False MyMethod MyAction Unknown";
            Assert.Equal(expectedMessage, message);
        }
    }
}
