using MedicalExaminer.Common.Extensions.Permission;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.ExtensionMethods
{
    public class MePermissionExtensionTests
    {
        [Fact]
        public void MePermissionIdentical_Returns_True()
        {
            // Arrange
            var permissionA = new MEUserPermission
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer
            };

            var permissionB = new MEUserPermission
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer
            };

            // Assert
            Assert.True(permissionA.IsEquivalent(permissionB));
        }

        [Fact]
        public void MePermissionDifferentLocationId_Returns_False()
        {
            // Arrange
            var permissionA = new MEUserPermission
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer
            };

            var permissionB = new MEUserPermission
            {
                LocationId = "locationId-bananas",
                UserRole = UserRoles.MedicalExaminer
            };

            // Assert
            Assert.False(permissionA.IsEquivalent(permissionB));
        }

        [Fact]
        public void MePermissionDifferentRole_Returns_False()
        {
            // Arrange
            var permissionA = new MEUserPermission
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer
            };

            var permissionB = new MEUserPermission
            {
                LocationId = "locationId",
                UserRole = UserRoles.ServiceAdministrator
            };

            // Assert
            Assert.False(permissionA.IsEquivalent(permissionB));
        }
    }
}