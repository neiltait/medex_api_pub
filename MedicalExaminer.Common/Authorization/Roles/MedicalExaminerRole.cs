using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization.Roles
{
    /// <summary>
    /// Medical Examiner Role.
    /// </summary>
    public class MedicalExaminerRole : Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="MedicalExaminerRole"/>.
        /// </summary>
        public MedicalExaminerRole()
            : base(UserRoles.MedicalExaminer)
        {
            Grant(
                Permission.GetLocations,
                Permission.GetLocation,
                Permission.GetExaminations,
                Permission.GetExamination,
                Permission.UpdateExamination,
                Permission.UpdateExaminationState,
                Permission.AddEventToExamination,
                Permission.GetExaminationEvents,
                Permission.GetExaminationEvent,
                Permission.GetProfile,
                Permission.UpdateProfile,
                Permission.BereavedDiscussionEvent,
                Permission.QapDiscussionEvent,
                Permission.OtherEvent,
                Permission.PreScrutinyEvent);
        }
    }
}
