namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Permission.
    /// </summary>
    public enum Permission
    {
        GetUsers,
        GetUser,
        InviteUser,
        SuspendUser,
        EnableUser,
        DeleteUser,
        GetUserPermissions,
        GetUserPermission,
        CreateUserPermission,
        UpdateUserPermission,
        DeleteUserPermission,

        GetLocations,
        GetLocation,

        GetExaminations,
        GetExamination,

        CreateExamination,
        AssignExaminationToMedicalExaminer,
        UpdateExamination,
        UpdateExaminationState,

        AddEventToExamination,
        GetExaminationEvents,
        GetExaminationEvent,

        GetProfile,
        UpdateProfile,
        GetProfilePermissions,
    }
}
