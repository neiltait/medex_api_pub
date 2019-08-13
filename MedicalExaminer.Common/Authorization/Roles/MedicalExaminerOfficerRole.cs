using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization.Roles
{
    /// <summary>
    /// Medical Examiner Officer Role.
    /// </summary>
    public class MedicalExaminerOfficerRole : Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="MedicalExaminerOfficerRole"/>.
        /// </summary>
        public MedicalExaminerOfficerRole()
            : base(UserRoles.MedicalExaminerOfficer)
        {
            Grant(
                Permission.GetLocations,
                Permission.GetLocation,
                Permission.GetExaminations,
                Permission.GetExamination,
                Permission.CreateExamination,
                Permission.AssignExaminationToMedicalExaminer,
                Permission.UpdateExamination,
                Permission.UpdateExaminationState,
                Permission.AddEventToExamination,
                Permission.GetExaminationEvents,
                Permission.GetExaminationEvent,
                Permission.GetProfile,
                Permission.UpdateProfile,
                Permission.GetProfilePermissions,
                Permission.BereavedDiscussionEvent,
                Permission.QapDiscussionEvent,
                Permission.MeoSummaryEvent,
                Permission.OtherEvent,
                Permission.AdmissionEvent,
                Permission.MedicalHistoryEvent,
                Permission.GetCoronerReferralDownload);
        }
    }
}
