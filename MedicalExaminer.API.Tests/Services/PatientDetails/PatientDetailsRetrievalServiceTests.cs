using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

            IDatabaseAccess dataAccess = new DatabaseAccess();

            

            //var connectionSettings = new ExaminationConnectionSettings(new Uri(url), primaryKey, databaseId);
            //var service = new PatientDetailsRetrievalService(dataAccess, connectionSettings);

            //var z = dataAccess
            //    .GetItemsAsync<MedicalExaminer.Models.Examination>
            //    (connectionSettings,
            //        xxxx => xxxx.PatientDetails.GivenNames == mystring);

            //var y = dataAccess.GetItemAsync<MedicalExaminer.Models.Examination>(connectionSettings, mystring);

            //Expression<Func<MedicalExaminer.Models.Examination, bool>> t = (x) => x.PatientDetails.Surname == mystring;
            
            //var xx = dataAccess.GetItemsAsync<MedicalExaminer.Models.Examination,
            //    MedicalExaminer.Models.PatientDetails>(connectionSettings, expression => expression.);

            // //await dataAccess.GetItemPartAsync<MedicalExaminer.Models.Examination,<MedicalExaminer.Models.PatientDetails>(
            // //   connectionSettings, 
            // //   exam => exam.PatientDetails.Surname == "");

            //var result = service.Handle(new PatientDetailsByCaseIdQuery(mystring)).Result;

            //var name = result.Surname;
        }
    }
}
