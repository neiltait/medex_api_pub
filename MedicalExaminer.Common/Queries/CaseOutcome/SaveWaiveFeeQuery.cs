using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    /// <summary>
    /// A query object to update the waiveFee property on the examination
    /// </summary>
    public class SaveWaiveFeeQuery : IQuery<string>
    {
        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="examinationId">The Examination ID</param>
        /// <param name="user">The user that called the endpoint</param>
        /// <param name="waiveFee">Should the fee be waived?</param>
        public SaveWaiveFeeQuery(string examinationId, MeUser user, bool waiveFee)
        {
            ExaminationId = examinationId;
            User = user;
            WaiveFee = waiveFee;
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
        /// Should the fee be waived?
        /// </summary>
        public bool WaiveFee { get; }
    }
}
