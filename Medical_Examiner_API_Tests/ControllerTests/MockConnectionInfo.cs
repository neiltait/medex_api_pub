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
    public class MockConnectionInfo : ConnectionInfo
    {
        public override string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IPAddress RemoteIpAddress
        {
            get { return null; } set { }
        }

        /// <inheritdoc/>
        public override int RemotePort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public override IPAddress LocalIpAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public override int LocalPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
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
}
