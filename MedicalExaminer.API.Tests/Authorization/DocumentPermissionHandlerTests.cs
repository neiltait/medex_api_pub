using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Xunit;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Tests.Authorization
{
    /// <summary>
    /// Document Permission Handler Tests.
    /// </summary>
    public class DocumentPermissionHandlerTests
    {
        private readonly Mock<IPermissionService> _permissionServiceMock;

        private readonly DocumentPermissionHandler _sut;

        public DocumentPermissionHandlerTests()
        {
            _permissionServiceMock = new Mock<IPermissionService>();

            _sut = new DocumentPermissionHandler(_permissionServiceMock.Object);
        }

        [Fact]
        public async void HandleRequirementAsync_Succeeded()
        {
            // Arrange
            var expectedEmail = "test@example.com";
            var expectedPermission = Permission.AddEventToExamination;
            var expectedLocation = "expectedLocation";
            var requirements = new List<IAuthorizationRequirement>()
            {
                new PermissionRequirement(expectedPermission)
            };
            var claim = new Claim(ClaimTypes.Email, expectedEmail);
            var user = new TestPrincipal(claim);
            var resource = new TestDocument()
            {
                NationalLocationId = expectedLocation,
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);

            _permissionServiceMock
                .Setup(ps => ps.HasPermission(expectedEmail, resource, expectedPermission))
                .Returns(Task.FromResult(true));

            // Act
            await _sut.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async void HandleRequirementAsync_WithNoPermissions_Failed()
        {
            // Arrange
            var expectedEmail = "test@example.com";
            var expectedPermission = Permission.AddEventToExamination;
            var expectedLocation = "expectedLocation";
            var requirements = new List<IAuthorizationRequirement>()
            {
                new PermissionRequirement(expectedPermission)
            };
            var claim = new Claim(ClaimTypes.Email, expectedEmail);
            var user = new TestPrincipal(claim);
            var resource = new TestDocument()
            {
                NationalLocationId = expectedLocation,
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);

            _permissionServiceMock
                .Setup(ps => ps.HasPermission(expectedEmail, resource, expectedPermission))
                .Returns(Task.FromResult(false));

            // Act
            await _sut.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeFalse();
        }

        /// <summary>
        /// Test Principal for mocking claims.
        /// </summary>
        public class TestPrincipal : ClaimsPrincipal
        {
            /// <summary>
            /// Inialise a new instance of the Test Principal.
            /// </summary>
            /// <param name="claims">Claims.</param>
            public TestPrincipal(params Claim[] claims)
                : base(new TestIdentity(claims))
            {
            }
        }

        /// <summary>
        /// Test Identity for mocking claims.
        /// </summary>
        public class TestIdentity : ClaimsIdentity
        {
            /// <summary>
            /// Initialise a new instance of the Test Identiy.
            /// </summary>
            /// <param name="claims">Claims.</param>
            public TestIdentity(params Claim[] claims)
                : base(claims)
            {
            }
        }

        private class TestDocument : ILocationBasedDocument
        {
            public string NationalLocationId { get; set; }
            public string RegionLocationId { get; set; }
            public string TrustLocationId { get; set; }
            public string SiteLocationId { get; set; }
        }

    }
}
