using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Extensions.ApplicationBuilder;
using MedicalExaminer.API.Tests.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Extensions.ApplicationBuilder
{
    public class UseProductVersionInAllResponseHeadersExtensionTests
    {
        [Fact]
        public void UseProductVersionInAllResponseHeadersExtension_AddsVersionHeader()
        {
            // Arrange
            var appMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);
            var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
            var dict = new HeaderDictionary();

            httpContext
                .SetupGet(c => c.Response.Headers)
                .Returns(dict);

            appMock
                .Setup(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                .Returns((Func<RequestDelegate, RequestDelegate> middleware) =>
                {
                    var a = middleware(context => null);

                    a(httpContext.Object);

                    return appMock.Object;
                })
                .Verifiable();

            // Act
            appMock.Object.UseProductVersionInAllResponseHeaders();

            // Assert
            dict.ContainsKey(UseProductVersionInAllResponseHeadersExtension.VersionKey).Should().BeTrue();
        }
    }
}
