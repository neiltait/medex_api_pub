using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries
{
    public class CreateExaminationQuery : IQuery<string>
    {
        public Examination Examination { get; }
        public CreateExaminationQuery(Examination examination)
        {
            Examination = examination;
        }
    }
}
