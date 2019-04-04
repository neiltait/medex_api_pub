using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
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
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> _userRetrievalServiceMock;

        private readonly Mock<IRolePermissions> _rolePermissionsMock;

        private readonly DocumentPermissionHandler _sut;

        public DocumentPermissionHandlerTests()
        {
            _rolePermissionsMock = new Mock<IRolePermissions>();

            _userRetrievalServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();

            _sut = new DocumentPermissionHandler(
                _rolePermissionsMock.Object, 
                _userRetrievalServiceMock.Object);
        }

        [Fact]
        public async void HandleRequirementAsync_Succeeded()
        {
            // Arrange
            var expectedLocation = "expectedLocation";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;
            var requirements = new List<IAuthorizationRequirement>()
            {
                new PermissionRequirement(expectedPermission)
            };
            var claim = new Claim(ClaimTypes.Email, "test@example.com");
            var user = new TestPrincipal(claim);
            var resource = new TestDocument()
            {
                NationalLocationId = expectedLocation,
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        PermissionId = "",
                        LocationId = expectedLocation,
                        UserRole = (int)expectedRole,
                    }
                }
            };

            _rolePermissionsMock
                .Setup(rp => rp.Can(expectedRole, expectedPermission))
                .Returns(true);

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            await _sut.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async void HandleRequirementAsync_WithNoPermissions_Failed()
        {
            // Arrange
            var expectedLocation = "expectedLocation";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;
            var requirements = new List<IAuthorizationRequirement>()
            {
                new PermissionRequirement(expectedPermission)
            };
            var claim = new Claim(ClaimTypes.Email, "test@example.com");
            var user = new TestPrincipal(claim);
            var resource = new TestDocument()
            {
                NationalLocationId = expectedLocation,
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        PermissionId = "",
                        LocationId = expectedLocation,
                        UserRole = (int)expectedRole,
                    }
                }
            };

            _rolePermissionsMock
                .Setup(rp => rp.Can(expectedRole, expectedPermission))
                .Returns(false);

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            await _sut.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeFalse();
        }

        [Fact]
        public async void HandleRequirementAsync_WithPermissionsButNotCorrect_Failed()
        {
            // Arrange
            var expectedLocation = "expectedLocation";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;
            var requirements = new List<IAuthorizationRequirement>()
            {
                new PermissionRequirement(expectedPermission)
            };
            var claim = new Claim(ClaimTypes.Email, "test@example.com");
            var user = new TestPrincipal(claim);
            var resource = new TestDocument()
            {
                NationalLocationId = expectedLocation,
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var meUser = new MeUser()
            {
                Permissions = null,
            };

            _rolePermissionsMock
                .Setup(rp => rp.Can(expectedRole, expectedPermission))
                .Returns(false);

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

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
