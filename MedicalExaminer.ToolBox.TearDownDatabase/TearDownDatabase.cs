using System;
using System.IO;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.ToolBox.Common.Dtos.Generate;
using MedicalExaminer.ToolBox.Common.Services;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.ToolBox.TearDownDatabase
{
    /// <summary>
    /// Tear Down Database
    /// </summary>
    internal class TearDownDatabase
    {
        private readonly ILogger _logger;

        private readonly IDocumentClientFactory _service;
        private readonly IOptions<CosmosDbSettings> _settings;

        /// <summary>
        /// Initialise a new instance of <see cref="TearDownDatabase"/>.
        /// </summary>
        /// <param name="logger">Logger to output to.</param>
        /// <param name="service">Service to import documents.</param>
        /// <param name="settings">Seed settings.</param>
        public TearDownDatabase(ILogger<TearDownDatabase> logger, IDocumentClientFactory service, IOptions<CosmosDbSettings> settings)
        {
            _logger = logger;
            _service = service;
            _settings = settings;
        }

        /// <summary>
        /// Seed
        /// </summary>
        /// <returns></returns>
        public async Task TearDown()
        {
            _logger.LogInformation("Tearing Down Database...");

            var connectionSettings = new LocationConnectionSettings(
                new Uri( _settings.Value.URL),
                _settings.Value.PrimaryKey,
                _settings.Value.DatabaseId);

            var client = _service.CreateClient(connectionSettings, false);

            try
            {
                await client.DeleteDatabaseAsync($"dbs/{_settings.Value.DatabaseId}");
            }
            catch (DocumentClientException documentClientException)
            {
                _logger.LogError(documentClientException.Message);
                _logger.LogError(documentClientException.StackTrace);
            }

            _logger.LogInformation($" Torn down.");
        }
    }
}
