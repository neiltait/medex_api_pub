using MedicalExaminer.Models;
using System;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class ConfirmationOfScrutinyQuery : IQuery<Models.Examination>
    {
        public ConfirmationOfScrutinyQuery(string examinationId, MeUser user)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                throw new ArgumentNullException(nameof(examinationId));
            }

            ExaminationId = examinationId;
            User = user ?? throw new ArgumentException(nameof(user));
        }

        public string ExaminationId { get; }

        public MeUser User { get; }
    }
}
