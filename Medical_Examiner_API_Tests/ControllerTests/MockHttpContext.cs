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
    public class MockHttpContext : HttpContext
    {
        private ClaimsPrincipal _claimsPrincipal;
        private ConnectionInfo _connectionInfo;

        public MockHttpContext()
            : base()
        {
            _claimsPrincipal = new ClaimsPrincipal();
            _connectionInfo = new MockConnectionInfo();
        }

        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest Request => throw new NotImplementedException();

        public override HttpResponse Response => throw new NotImplementedException();

        public override ConnectionInfo Connection
        {
            get { return _connectionInfo; }
        }

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override ClaimsPrincipal User
        {
            get { return _claimsPrincipal; } set { _claimsPrincipal = value; }
        }

        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [Obsolete("Microsoft.AspNetCore.Http.Authentication.AuthenticationManager Authentication is deprecated, CS0672 warning")]
        public override Microsoft.AspNetCore.Http.Authentication.AuthenticationManager Authentication
        {
            get
            {
                throw new NotImplementedException();
            }
        }

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
}
