namespace MedicalExaminer.Models
{
    public static class MeUserPermissionExtensions
    {
        public static bool IsSame(this MEUserPermission permissionA, MEUserPermission permissionB)
        {
            return permissionA.LocationId == permissionB.LocationId && permissionA.UserRole == permissionB.UserRole;
        }
    }
}
