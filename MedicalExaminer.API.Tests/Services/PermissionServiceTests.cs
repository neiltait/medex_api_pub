using System;
using System.Collections.Generic;
using System.Linq;
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
        public async void HasPermission_ForDocument_ReturnsTrue_WhenRolePermissionReturnsTrue()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;
            var expectedNationalLocationId = "expectedNationalLocationId";

            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        PermissionId = "",
                        LocationId = expectedNationalLocationId,
                        UserRole = (int)expectedRole,
                    }
                }
            };

            var document = new Mock<ILocationBasedDocument>();

            document.Setup(d => d.NationalLocationId).Returns(expectedNationalLocationId);

            _rolePermissionsMock
                .Setup(rp => rp.Can(expectedRole, expectedPermission))
                .Returns(true);

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            var result = await _sut.HasPermission(expectedEmail, document.Object, expectedPermission);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void HasPermission_ForDocument_ReturnsFalse_WhenRolePermissionReturnsFalse()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;
            var expectedNationalLocationId = "expectedNationalLocationId";

            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        PermissionId = "",
                        LocationId = expectedNationalLocationId,
                        UserRole = (int)expectedRole,
                    }
                }
            };

            var document = new Mock<ILocationBasedDocument>();

            document.Setup(d => d.NationalLocationId).Returns(expectedNationalLocationId);

            _rolePermissionsMock
                .Setup(rp => rp.Can(expectedRole, expectedPermission))
                .Returns(false);

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            var result = await _sut.HasPermission(expectedEmail, document.Object, expectedPermission);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void HasPermission_ForDocument_ReturnsFalse_WhenUserHasNoPermissions()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;

            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                }
            };

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            var result = await _sut.HasPermission(expectedEmail, null, expectedPermission);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void HasPermission_ReturnsTrue_WhenRolePermissionReturnsTrue()
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

        [Fact]
        public async void HasPermission_ReturnsFalse_WhenRolePermissionReturnsFalse()
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
                .Returns(false);

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            var result = await _sut.HasPermission(expectedEmail, expectedPermission);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void HasPermission_ReturnsFalse_WhenUserHasNoPermissions()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedPermission = Permission.AddEventToExamination;

            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
            };

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            var result = await _sut.HasPermission(expectedEmail, expectedPermission);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void LocationIdsWithPermission_ReturnsLocations()
        {
            // Arrange
            var expectedRole = UserRoles.MedicalExaminer;
            var unexpectedRole = UserRoles.MedicalExaminerOfficer;
            var expectedPermission = Permission.AddEventToExamination;
            var expectedLocation = "expectedLocation";
            var unexpectedLocation = "unexpectedLocation";
            var meUser = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        PermissionId = string.Empty,
                        LocationId = expectedLocation,
                        UserRole = (int)expectedRole,
                    },
                    new MEUserPermission()
                    {
                        PermissionId = string.Empty,
                        LocationId = unexpectedLocation,
                        UserRole = (int)unexpectedRole,
                    }
                }
            };

            _rolePermissionsMock
                .Setup(rp => rp.Can(expectedRole, expectedPermission))
                .Returns(true);

            _rolePermissionsMock
                .Setup(rp => rp.Can(unexpectedRole, expectedPermission))
                .Returns(false);

            _userRetrievalServiceMock
                .Setup(urs => urs.Handle(It.IsAny<UserRetrievalByEmailQuery>()))
                .Returns(Task.FromResult(meUser));

            // Act
            var actualLocations = _sut.LocationIdsWithPermission(meUser, expectedPermission).ToList();

            // Assert
            actualLocations.Contains(expectedLocation).Should().BeTrue();
            actualLocations.Contains(unexpectedLocation).Should().BeFalse();
        }
    }
}
