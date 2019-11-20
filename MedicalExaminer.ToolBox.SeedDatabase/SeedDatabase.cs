using System.IO;
using System.Threading.Tasks;
using MedicalExaminer.ToolBox.Common.Dtos.Generate;
using MedicalExaminer.ToolBox.Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.ToolBox.SeedDatabase
{
    /// <summary>
    /// Seed Database
    /// </summary>
    internal class SeedDatabase
    {
        private readonly ILogger _logger;

        private readonly ImportDocumentService _service;
        private readonly SeedDatabaseSettings _settings;

        /// <summary>
        /// Initialise a new instance of <see cref="SeedDatabase"/>.
        /// </summary>
        /// <param name="logger">Logger to output to.</param>
        /// <param name="service">Service to import documents.</param>
        /// <param name="settings">Seed settings.</param>
        public SeedDatabase(ILogger<SeedDatabase> logger, ImportDocumentService service, IOptions<SeedDatabaseSettings> settings)
        {
            _logger = logger;
            _service = service;
            _settings = settings.Value;
        }

        /// <summary>
        /// Seed
        /// </summary>
        /// <returns></returns>
        public async Task Seed()
        {
            _logger.LogInformation("Importing models...");

            _logger.LogInformation($"Importing users from: {_settings.ImportUsersFrom}");

            var countUsers = 0;

            foreach (var file in Directory.EnumerateFiles(_settings.ImportUsersFrom, "*.json"))
            {
                var contents = File.ReadAllText(file);

                var model = await _service.ImportUser(contents);

                countUsers++;

                _logger.LogInformation($" Imported user with id: {model.UserId}");
            }

            _logger.LogInformation($" Total:{countUsers} ");

            _logger.LogInformation($"Importing users from: {_settings.ImportLocationsFrom}");

            var countLocations = 0;

            foreach (var file in Directory.EnumerateFiles(_settings.ImportLocationsFrom, "*.json"))
            {
                var contents = File.ReadAllText(file);

                var model = await _service.ImportLocation(contents);

                countLocations++;

                _logger.LogInformation($" Imported location with id: {model.LocationId}");
            }

            _logger.LogInformation($" Total:{countLocations} ");

            _logger.LogInformation("Done.");
        }
    }
}
