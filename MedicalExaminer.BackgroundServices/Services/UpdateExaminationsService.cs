using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.BackgroundServices.Services
{
    /// <summary>
    /// Update Examinations Service.
    /// </summary>
    public class UpdateExaminationsService : ScheduledService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialise a new instance of <see cref="UpdateExaminationsService"/>.
        /// </summary>
        /// <param name="configuration">Scheduled service configuration.</param>
        /// <param name="serviceProvider"></param>
        public UpdateExaminationsService(IScheduledServiceConfiguration configuration, IServiceProvider serviceProvider)
            : base(configuration)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var examinationsUpdateCaseUrgencyScoreService = scope.ServiceProvider.GetRequiredService<IAsyncQueryHandler<ExaminationsUpdateCaseUrgencyScoreQuery, int>>();

                    Console.WriteLine("Running!!");

                    var result = await examinationsUpdateCaseUrgencyScoreService.Handle(
                        new ExaminationsUpdateCaseUrgencyScoreQuery(new MeUser()));

                    Console.WriteLine($"Records updated: {result}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"oops: {e}");
                }
            }
        }
    }
}
