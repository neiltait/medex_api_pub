using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Models.Enums;
using Moq;
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
