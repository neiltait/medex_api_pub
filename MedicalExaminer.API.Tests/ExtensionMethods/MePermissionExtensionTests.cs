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
            var permissionA = new MEUserPermission()
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer,
                PermissionId = "permissionId"
            };

            var permissionB = new MEUserPermission()
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer,
                PermissionId = "permissionId"
            };

            Assert.True(permissionA.IsSame(permissionB));
        }

        [Fact]
        public void MePermissionDifferentLocationId_Returns_False()
        {
            var permissionA = new MEUserPermission()
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer,
                PermissionId = "permissionId"
            };

            var permissionB = new MEUserPermission()
            {
                LocationId = "locationId-bananas",
                UserRole = UserRoles.MedicalExaminer,
                PermissionId = "permissionId"
            };

            Assert.False(permissionA.IsSame(permissionB));
        }

        [Fact]
        public void MePermissionDifferentRole_Returns_False()
        {
            var permissionA = new MEUserPermission()
            {
                LocationId = "locationId",
                UserRole = UserRoles.MedicalExaminer,
                PermissionId = "permissionId"
            };

            var permissionB = new MEUserPermission()
            {
                LocationId = "locationId",
                UserRole = UserRoles.ServiceAdministrator,
                PermissionId = "permissionId"
            };

            Assert.False(permissionA.IsSame(permissionB));
        }
    }

    //move to correct place
    // on update permission, check against the users current permissions that the updated one isn't the same
    //  on insert new permission do the same

    //  happy?

    //public static class MeUserPermissionExtensions
    //{
    //    public static bool IsSame(this MEUserPermission permissionA, MEUserPermission permissionB)
    //    {
    //        return permissionA.LocationId == permissionB.LocationId && permissionA.UserRole == permissionB.UserRole;
    //    }
    //}
}