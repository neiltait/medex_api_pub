using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries
{
    public class ExaminationRetrivalQuery : IQuery<Examination>
    {
        public Guid ExaminationId { get; }
        public ExaminationRetrivalQuery(Guid examinationId)
        {
            ExaminationId = examinationId;
        }
    }
}
