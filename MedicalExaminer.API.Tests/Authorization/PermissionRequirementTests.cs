using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.Common.Authorization;
using Xunit;

namespace MedicalExaminer.API.Tests.Authorization
{
    public class PermissionRequirementTests
    {
        [Fact]
        public void Permission_ReturnsPermission()
        {
            // Arrange
            const Permission permission = Permission.CreateUserPermission;

            // Act
            var sut = new PermissionRequirement(permission);

            // Assert
            sut.Permission.Should().Be(permission);
        }
    }
}
