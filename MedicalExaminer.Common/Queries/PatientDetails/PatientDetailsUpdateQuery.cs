using System.Collections;
using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.PatientDetails
{
    public class PatientDetailsUpdateQuery : IQuery<Models.Examination>
    {
        public PatientDetailsUpdateQuery(string caseId, Models.PatientDetails patientDetails, MeUser user, IEnumerable<Models.Location> locations)
        {
            CaseId = caseId;
            PatientDetails = patientDetails;
            User = user;
            Locations = locations;
        }

        public string CaseId { get; }

        public MeUser User { get; }

        public Models.PatientDetails PatientDetails { get; }

        public IEnumerable<Models.Location> Locations { get; }
    }
}