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
        UpdateUser,
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

        BereavedDiscussionEvent,
        MeoSummaryEvent,
        QapDiscussionEvent,
        OtherEvent,
        AdmissionEvent,
        MedicalHistoryEvent,
        PreScrutinyEvent,
        GetCoronerReferralDownload,
    }
}
