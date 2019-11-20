using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class VoidCaseQuery : IQuery<Models.Examination>
    {
        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="examinationId">The Examination ID</param>
        /// <param name="user">The user that called the endpoint</param>
        /// <param name="voidReason">Reason for voiding the case</param>
        public VoidCaseQuery(string examinationId, MeUser user, string voidReason)
        {
            ExaminationId = examinationId;
            User = user;
            VoidReason = voidReason;
        }

        /// <summary>
        /// The examinations ID
        /// </summary>
        public string ExaminationId { get; }

        /// <summary>
        /// The user that called the endpoint
        /// </summary>
        public MeUser User { get; }

        /// <summary>
        /// Reason for voiding the examination
        /// </summary>
        public string VoidReason { get; set; }
    }
}
