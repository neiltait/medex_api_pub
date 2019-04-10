using System.Collections.Generic;
using FluentAssertions;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.Common.Tests.Authorization
{
    /// <summary>
    /// Role Permissions Tests.
    /// </summary>
    public class RolePermissionsTests
    {
        /// <summary>
        /// Can Return True.
        /// </summary>
        [Fact]
        public void Can_ReturnTrue()
        {
            // Arrange
            var role = new TestRole();
            var sut = new RolePermissions(new List<Role> { role });

            // Act
            var result = sut.Can(UserRoles.MedicalExaminer, Permission.AddEventToExamination);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Can Return False.
        /// </summary>
        [Fact]
        public void Can_ReturnFalse()
        {
            // Arrange
            var role = new TestRole();
            var sut = new RolePermissions(new List<Role> { role });

            // Act
            var result = sut.Can(UserRoles.MedicalExaminer, Permission.CreateUserPermission);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Can Return False.
        /// </summary>
        [Fact]
        public void Can_ReturnFalse_WhenException()
        {
            // Arrange
            var role = new TestRole();
            var sut = new RolePermissions(new List<Role> { role });

            // Act
            var result = sut.Can(UserRoles.MedicalExaminerOfficer, Permission.CreateUserPermission);

            // Assert
            result.Should().BeFalse();
        }

        private class TestRole : Role
        {
            public TestRole()
                : base(UserRoles.MedicalExaminer)
            {
                Grant(
                    Permission.AddEventToExamination,
                    Permission.AssignExaminationToMedicalExaminer);
            }
        }
    }
}
