using System;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Location;
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
            var connectionSettings = new LocationConnectionSettings(new Uri("https://medical-examiners-sandbox.documents.azure.com:443/"),
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
