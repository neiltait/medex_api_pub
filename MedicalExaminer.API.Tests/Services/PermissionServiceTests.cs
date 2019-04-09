using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Services;
using MedicalExaminer.API.Services.Implementations;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Tests.Services
{
    public class PermissionServiceTests
    {
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>> _userRetrievalServiceMock;

        private readonly Mock<IRolePermissions> _rolePermissionsMock;

        private readonly IPermissionService _sut;

        public PermissionServiceTests()
        {
            _rolePermissionsMock = new Mock<IRolePermissions>();

            _userRetrievalServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();

            _sut = new PermissionService(
                _rolePermissionsMock.Object,
                _userRetrievalServiceMock.Object);
        }

        [Fact]
        public async void HasPermission_()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;

            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        PermissionId = "",
                        LocationId = "",
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
            var result = await _sut.HasPermission(expectedEmail, expectedPermission);

            // Assert
            result.Should().BeTrue();
        }

/*
        [Fact]
        public async void HandleRequirementAsync_WithNoPermissions_Failed()
        {
            // Arrange
            var expectedPermission = Permission.AddEventToExamination;
            var requirements = new List<IAuthorizationRequirement>()
            {
                new PermissionRequirement(expectedPermission)
            };
            var claim = new Claim(ClaimTypes.Email, "test@example.com");
            var user = new TestPrincipal(claim);
            const object resource = (object)null;
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var meUser = new MeUser()
            {
                Permissions = null
            };

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
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;
            var requirements = new List<IAuthorizationRequirement>()
            {
                new PermissionRequirement(expectedPermission)
            };
            var claim = new Claim(ClaimTypes.Email, "test@example.com");
            var user = new TestPrincipal(claim);
            const object resource = (object)null;
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        PermissionId = "",
                        LocationId = "",
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
         */
    }
}
