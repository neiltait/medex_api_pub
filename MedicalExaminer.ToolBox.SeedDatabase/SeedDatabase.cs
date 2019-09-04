using System.Threading.Tasks;
using MedicalExaminer.ToolBox.Common.Dtos.Generate;
using MedicalExaminer.ToolBox.Common.Services;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.ToolBox.SeedDatabase
{
    class SeedDatabase
    {
        private readonly ILogger _logger;

        private readonly GenerateConfigurationService _service;

        public SeedDatabase(ILogger<SeedDatabase> logger, GenerateConfigurationService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task Seed()
        {
            _logger.LogInformation("Generating...");

            await _service.Generate(new GenerateConfiguration());

            _logger.LogInformation("Done.");
        }
    }
}
