using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreateOtherEventQuery : IQuery<string>
    {
        public Models.EventNote EventNote { get; }

        public CreateOtherEventQuery(Models.EventNote eventNote)
        {
            EventNote = eventNote;
        }
    }
}
