using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
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
        /// <param name="serviceProvider">Service provider.</param>
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
                var examinationStore =
                    (ICosmosStore<Examination>) scope.ServiceProvider.GetService(typeof(ICosmosStore<Examination>));

                Console.WriteLine("Running!!");

                var examinations = await examinationStore.Query().Where(e => !e.CaseCompleted).ToListAsync(cancellationToken);

                foreach (var examination in examinations)
                {
                    examination.UpdateCaseUrgencyScore();
                }

                Console.WriteLine($"Updated {examinations.Count}");
                await examinationStore.UpdateRangeAsync(examinations, null, cancellationToken);
            }
        }
    }
}
