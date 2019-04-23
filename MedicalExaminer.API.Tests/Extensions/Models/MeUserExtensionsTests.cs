using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Models;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Extensions.Models
{
    public class MeUserExtensionsTests
    {
        [Fact]
        public void RoleForExamination_Returns()
        {
            // Arrange
            var expectedRole = UserRoles.ServiceOwner;
            var user = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        LocationId = "site1",
                        UserRole = (int) UserRoles.MedicalExaminerOfficer
                    },
                    new MEUserPermission()
                    {
                        LocationId = "trust1",
                        UserRole = (int) UserRoles.MedicalExaminer
                    },
                    new MEUserPermission()
                    {
                        LocationId = "national1",
                        UserRole = (int) expectedRole
                    }
                }
            };

            var examination = new Examination()
            {
                NationalLocationId = "national1",
                RegionLocationId = "region1",
                TrustLocationId = "trust1",
                SiteLocationId = "site1"
            };

            // Act
            var role = user.RoleForExamination(examination);

            // Assert
            role.Should().Be(expectedRole);
        }
    }
}
