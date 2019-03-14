using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Services;
using MedicalExaminer.API.Services.Implementations;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Implementations
{
    public class OktaTokenServiceTests
    {
        private readonly IOptions<OktaSettings> _oktaSettings;

        private readonly OktaTokenService _sut;

        public OktaTokenServiceTests()
        {
            _oktaSettings = Options.Create(new OktaSettings()
            {
                Authority = "authority",
                Audience = "audience",
                ClientId = "clientId",
                ClientSecret = "clientSecret",
                Domain = "domain",
                IntrospectUrl = "http://www.example.com/",
                SdkToken = "token",
            });

            _sut = new OktaTokenService(_oktaSettings);
        }

        [Fact]
        public void IntrospectToken_ShouldReturnIntrospectResponse_WhenValidToken()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var token = "token";

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'active':false}"),
                })
                .Verifiable();

            var result = _sut.IntrospectToken(token, httpClient);

            result.Result.Should().BeOfType<IntrospectResponse>();
            result.Result.Active.Should().BeFalse();
        }

        [Fact]
        public async Task IntrospectToken_ShouldThrowException_WhenResponseNotSuccessful()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var token = "token";

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                });

            await Assert.ThrowsAsync<ApplicationException>(async () =>
                await _sut.IntrospectToken(token, httpClient)
            );

        }
    }
}
