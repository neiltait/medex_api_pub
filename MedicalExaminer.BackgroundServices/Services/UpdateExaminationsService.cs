using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalExaminer.BackgroundServices.Services
{
    /// <summary>
    /// Update Examinations Service.
    /// </summary>
    public class UpdateExaminationsService : ScheduledService
    {
        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialise a new instance of <see cref="UpdateExaminationsService"/>.
        /// </summary>
        /// <param name="configuration">Scheduled service configuration.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public UpdateExaminationsService(IScheduledServiceConfiguration configuration, IScheduler scheduler, IServiceProvider serviceProvider)
            : base(configuration, scheduler)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var examinationStore = scope.ServiceProvider.GetRequiredService<ICosmosStore<Examination>>();

                var examinationAuditStore = scope.ServiceProvider.GetRequiredService<ICosmosStore<AuditEntry<Examination>>>();

                var examinations = await examinationStore
                    .Query()
                    .Where(e => !e.CaseCompleted)
                    .ToListAsync(cancellationToken);

                var examinationAudits = new List<AuditEntry<Examination>>();

                foreach (var examination in examinations)
                {
                    examination.UpdateCaseUrgencyScore();
                    examination.LastModifiedBy = "UpdateExaminationService";
                    examination.ModifiedAt = DateTimeOffset.UtcNow;

                    examinationAudits.Add(new AuditEntry<Examination>(examination));
                }

                Console.WriteLine($"Updated {examinations.Count}");
                await examinationStore.UpdateRangeAsync(examinations, null, cancellationToken);

                await examinationAuditStore.AddRangeAsync(examinationAudits, null, cancellationToken);
            }
        }
    }
}
