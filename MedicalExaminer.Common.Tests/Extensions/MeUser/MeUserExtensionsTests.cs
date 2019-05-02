using System.Collections.Generic;
using FluentAssertions;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.Common.Tests.Extensions.MeUser
{
    /// <summary>
    /// A class class.
    /// </summary>
    public class MeUserExtensionsTests
    {
        /// <summary>
        /// A test.
        /// </summary>
        [Fact]
        public void RoleForExamination_Returns()
        {
            // Arrange
            var expectedRole = UserRoles.ServiceOwner;
            var user = new Models.MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        LocationId = "site1",
                        UserRole = UserRoles.MedicalExaminerOfficer
                    },
                    new MEUserPermission()
                    {
                        LocationId = "trust1",
                        UserRole = UserRoles.MedicalExaminer
                    },
                    new MEUserPermission()
                    {
                        LocationId = "national1",
                        UserRole = expectedRole
                    }
                }
            };

            var examination = new Models.Examination()
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

        /// <summary>
        /// A nother test.
        /// </summary>
        [Fact]
        public void UsersExaminationRole_EmPtyRequiredRoles_Returns_Null()
        {
            // Arrange
            var expectedRole = UserRoles.MedicalExaminer;
            var user = new Models.MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        LocationId = "trust1",
                        UserRole = UserRoles.MedicalExaminer
                    },
                    new MEUserPermission()
                    {
                        LocationId = "national1",
                        UserRole = expectedRole
                    }
                }
            };

            List<UserRoles> requiredRoles = null;
            var examination = new Models.Examination()
            {
                NationalLocationId = "national1",
                RegionLocationId = "region1",
                TrustLocationId = "trust1",
                SiteLocationId = "site1"
            };

            // Act
            var role = user.UsersExaminationRole(requiredRoles);

            // Assert
            role.Should().BeNull();
        }

        /// <summary>
        /// A nother test.
        /// </summary>
        [Fact]
        public void UsersExaminationRole_RoleFound_Returns_Role()
        {
            // Arrange
            var expectedRole = UserRoles.MedicalExaminer;
            var user = new Models.MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        LocationId = "trust1",
                        UserRole = UserRoles.ServiceOwner
                    },
                    new MEUserPermission()
                    {
                        LocationId = "national1",
                        UserRole = expectedRole
                    }
                }
            };

            List<UserRoles> requiredRoles = new List<UserRoles>(2)
            {
                UserRoles.MedicalExaminer,
                UserRoles.MedicalExaminerOfficer
            };

            var examination = new Models.Examination()
            {
                NationalLocationId = "national1",
                RegionLocationId = "region1",
                TrustLocationId = "trust1",
                SiteLocationId = "site1"
            };

            // Act
            var role = user.UsersExaminationRole(requiredRoles);

            // Assert
            role.Should().Be(expectedRole);
        }

        /// <summary>
        /// A nother test.
        /// </summary>
        [Fact]
        public void UsersExaminationRole_RoleNotFound_Returns_Null()
        {
            // Arrange
            var user = new Models.MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        LocationId = "trust1",
                        UserRole = UserRoles.ServiceOwner
                    }
                }
            };

            List<UserRoles> requiredRoles = new List<UserRoles>(2)
            {
                UserRoles.MedicalExaminer,
                UserRoles.MedicalExaminerOfficer
            };

            var examination = new Models.Examination()
            {
                NationalLocationId = "national1",
                RegionLocationId = "region1",
                TrustLocationId = "trust1",
                SiteLocationId = "site1"
            };

            // Act
            var role = user.UsersExaminationRole(requiredRoles);

            // Assert
            role.Should().BeNull();
        }
    }
}
