using System;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services
{
    public class LocationIdServiceTests
    {
        [Fact]
        public void LocationIdNotFoundReturnsEmptyResultSet()
        {
            var dbAccess = new DatabaseAccess();
            var connectionSettings = new LocationConnectionSetting(new Uri("https://medical-examiners-sandbox.documents.azure.com:443/"),
                "***REMOVED***",
                "testing123"
                );
            var service = new LocationIdService(dbAccess, connectionSettings);
            var query = new LocationRetrivalByIdQuery("a");

            var result = service.Handle(query);
            var expected = default(Location);
            Assert.Equal(expected, result.Result);

        }
    }
}
