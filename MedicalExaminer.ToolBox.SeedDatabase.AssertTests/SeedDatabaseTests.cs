using System;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using FluentAssertions;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.ToolBox.SeedDatabase.AssertTests
{
    [Collection("EnvironmentVariables Collection")]
    public class SeedDatabaseTests
    {
        private const string CosmosDbUrl = "CosmosDB__URL";
        private const string CosmosDbPrimaryKey = "CosmosDB__PrimaryKey";
        private const string CosmosDbDatabaseId = "CosmosDB__DatabaseId";

        private readonly ICosmosStore<Examination> _examinationStore;

        public SeedDatabaseTests()
        {
            var databaseId = Environment.GetEnvironmentVariable(CosmosDbDatabaseId);
            var url = Environment.GetEnvironmentVariable(CosmosDbUrl);
            var primaeyKey = Environment.GetEnvironmentVariable(CosmosDbPrimaryKey);

            var cosmosStoreSettings = new CosmosStoreSettings(
                databaseId,
                url,
                primaeyKey
            );

            _examinationStore = new CosmosStore<Examination>(cosmosStoreSettings, "Examinations");

            // _examinationStore = 
        }

        [Fact]
        public async Task ExaminationsExist()
        {
            var examinations = await _examinationStore.Query()
                .Where(e => true)
                .ToListAsync();

            examinations.Count().Should().BeGreaterThan(0);
        }
    }
}
