using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Services.PatientDetails;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.PatientDetails
{
    public class PatientDetailsRetrievalServiceTests
    {
        [Fact]
        public void XXX()
        {
            var mystring = "bd58b2f3-6e37-4dae-946a-971ce7f58f0d";

            var url = "https://medical-examiners-sandbox.documents.azure.com:443/";
            var primaryKey = "***REMOVED***";
            var databaseId = "testing123-djp";

            var dataAccess = new DatabaseAccess();
            var connectionSettings = new ExaminationConnectionSettings(new Uri(url), primaryKey, databaseId);
            var service = new PatientDetailsRetrievalService(dataAccess, connectionSettings);

            var result = service.Handle(new PatientDetailsByCaseIdQuery(mystring)).Result;

            var name = result.Surname;
        }
    }
}
