using System.Collections.Generic;
using FluentAssertions;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.Common.Tests.Extensions.MeUser
{
    /// <summary>
    /// Me User Extension Tests.
    /// </summary>
    public class MeUserExtensionsTests
    {
        /// <summary>
        /// Role Returns Best Role for a User based on the Enum ordering.
        /// </summary>
        [Fact]
        public void Role_ReturnsBestRoleForUser_BasedOnEnumOrdering()
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

            // Act
            var role = user.Role();

            // Assert
            role.Should().Be(expectedRole);
        }

        /// <summary>
        /// Has Role for Examination returns true when user has required role.
        /// </summary>
        [Fact]
        public void HasRoleForExamination_ReturnsTrue_WhenUserHasRequiredRole()
        {
            // Arrange
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
                        UserRole = UserRoles.ServiceAdministrator,
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
            var hasRole = user.HasRoleForExamination(examination, UserRoles.MedicalExaminerOfficer);

            // Assert
            hasRole.Should().BeTrue();
        }

        /// <summary>
        /// Has Role for Examination returns false when user does not have required role.
        /// </summary>
        [Fact]
        public void HasRoleForExamination_ReturnsFalse_WhenUserDoesNotHaveRequiredRole()
        {
            // Arrange
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
                        UserRole = UserRoles.ServiceAdministrator,
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
            var hasRole = user.HasRoleForExamination(examination, UserRoles.MedicalExaminerOfficer);

            // Assert
            hasRole.Should().BeFalse();
        }

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
        public void UsersExaminationRole_EmptyRequiredRoles_Returns_Null()
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

            // Act
            var role = user.UsersRoleIn(requiredRoles);

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

            // Act
            var role = user.UsersRoleIn(requiredRoles);

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

            // Act
            var role = user.UsersRoleIn(requiredRoles);

            // Assert
            role.Should().BeNull();
        }

        /// <summary>
        /// A nother test.
        /// </summary>
        [Fact]
        public void UsersExaminationRole_UserHasNoPermissions_Returns_Null()
        {
            // Arrange
            var user = new Models.MeUser()
            {
                Permissions = null
            };

            List<UserRoles> requiredRoles = new List<UserRoles>(2)
            {
                UserRoles.MedicalExaminer,
                UserRoles.MedicalExaminerOfficer
            };

            // Act
            var role = user.UsersRoleIn(requiredRoles);

            // Assert
            role.Should().BeNull();
        }
    }
}
