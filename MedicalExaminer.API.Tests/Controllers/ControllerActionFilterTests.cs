using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Reporting;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;
using AuthenticationManager = Microsoft.AspNetCore.Http.Authentication.AuthenticationManager;

namespace MedicalExaminer.API.Tests.Controllers
{
    internal class MockConnectionInfo : ConnectionInfo
    {
        public override string Id
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override IPAddress RemoteIpAddress
        {
            get => null;
            set { }
        }

        public override int RemotePort
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override IPAddress LocalIpAddress
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override int LocalPort
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override X509Certificate2 ClientCertificate
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override Task<X509Certificate2> GetClientCertificateAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }

    internal class MockHttpContext : HttpContext
    {
        private ClaimsPrincipal _claimsPrincipal;

        public MockHttpContext()
        {
            _claimsPrincipal = new ClaimsPrincipal();
            Connection = new MockConnectionInfo();
        }

        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest Request => throw new NotImplementedException();

        public override HttpResponse Response => throw new NotImplementedException();

        public override ConnectionInfo Connection { get; }

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override AuthenticationManager Authentication { get; }

        public override ClaimsPrincipal User
        {
            get => _claimsPrincipal;
            set => _claimsPrincipal = value;
        }

        public override IDictionary<object, object> Items
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override IServiceProvider RequestServices
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override CancellationToken RequestAborted
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override string TraceIdentifier
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override ISession Session
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override void Abort()
        {
            throw new NotImplementedException();
        }
    }

    public class ControllerActionFilterTests
    {
        private readonly Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>> _mockLogger;
        private readonly UsersController _controller;
        private readonly Mock<IMapper> _mapper;

        public ControllerActionFilterTests()
        {
            _mockLogger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();

            _mapper = new Mock<IMapper>();
            var createUserService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
            var userRetrievalService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
            var usersRetrievalService =
                new Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>>();
            var userUpdateService = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>();
            var usersRetrievalByOktaIdServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var locationsService = new Mock<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var permissionServiceMock = new Mock<IPermissionService>();
            var locationsParentsServiceMock = new Mock<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>>();

            _controller = new UsersController(
                _mockLogger.Object,
                _mapper.Object,
                usersRetrievalByOktaIdServiceMock.Object,
                authorizationServiceMock.Object,
                permissionServiceMock.Object,
                createUserService.Object,
                userRetrievalService.Object,
                usersRetrievalService.Object,
                userUpdateService.Object,
                locationsService.Object,
                null,
                locationsParentsServiceMock.Object);
        }

        [Fact]
        public void OnActionExecuted_DoesNothing()
        {
            var controllerActionFilter = new ControllerActionFilter(_mockLogger.Object, new RequestChargeService());
            var actionContext = new ActionContext
            {
                HttpContext = new MockHttpContext(),
                ActionDescriptor = new ControllerActionDescriptor()
                {
                    ControllerName = "MyMethod",
                    ActionName = "MyAction"
                },
            };

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(MEClaimTypes.UserId, "UserId"));
            actionContext.HttpContext.User.AddIdentity(identity);
            actionContext.RouteData = new RouteData();
            var filters = new List<IFilterMetadata>();
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, filters, _controller);

            controllerActionFilter.OnActionExecuted(actionExecutedContext);
        }

        [Fact]
        public void OnActionExecuted_WritesLog()
        {
            var expectedUnknown = "Unknown";
            var expectedUserId = "expectedUserId";
            var expectedControllerName = "expectedControllerName";
            var expectedActionName = "expectedActionName";
            var controllerActionFilter = new ControllerActionFilter(_mockLogger.Object, new RequestChargeService());
            var actionContext = new ActionContext
            {
                HttpContext = new MockHttpContext(),
                ActionDescriptor = new ControllerActionDescriptor()
                {
                    ControllerName = expectedControllerName,
                    ActionName = expectedActionName
                }
            };

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(MEClaimTypes.UserId, expectedUserId));
            actionContext.HttpContext.User.AddIdentity(identity);
            actionContext.RouteData = new RouteData();

            var executingFilters = new List<IFilterMetadata>();
            var actionArguments = new Dictionary<string, object>();
            var actionExecutingContext =
                new ActionExecutingContext(actionContext, executingFilters, actionArguments, _controller);

            actionExecutingContext.ActionArguments.Add("filter", new { Property = "value" });

            var filters = new List<IFilterMetadata>();
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, filters, _controller);

            LogMessageActionDefault logEntry = null;
            _mockLogger
                .Setup(ml => ml.Handle(It.IsAny<CreateMELoggerQuery>()))
                .Callback((CreateMELoggerQuery query) => { logEntry = query.LogEntry; });

            controllerActionFilter.OnActionExecuting(actionExecutingContext);

            controllerActionFilter.OnActionExecuted(actionExecutedContext);

            logEntry.UserId.Should().Be(expectedUserId);
            logEntry.UserIsAuthenticated.Should().BeFalse();
            logEntry.ControllerName.Should().Be(expectedControllerName);
            logEntry.ControllerMethod.Should().Be(expectedActionName);
            logEntry.RemoteIP.Should().Be(expectedUnknown);
            logEntry.UserAuthenticationType.Should().Be(expectedUnknown);
            logEntry.Parameters.Keys.Should().Contain("filter");
        }
    }
}