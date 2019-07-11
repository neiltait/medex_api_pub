using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Cosmonaut.Testing;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
        /// <param name="logger">The Logger.</param>
        /// <param name="configuration">Scheduled service configuration.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public UpdateExaminationsService(ILogger<UpdateExaminationsService> logger, IScheduledServiceConfiguration configuration, IScheduler scheduler, IServiceProvider serviceProvider)
            : base(logger, configuration, scheduler)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.Log(LogLevel.Information, "Update examination background worker service running...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var examinationStore = scope.ServiceProvider.GetRequiredService<ICosmosStore<Examination>>();

                var examinationAuditStore = scope.ServiceProvider.GetRequiredService<ICosmosStore<AuditEntry<Examination>>>();

                var examinations = examinationStore
                    .Query()
                    .Where(e => !e.CaseCompleted)
                    .ToFeedResponse();

                var initialQueryCharge = examinations.RequestCharge;

                var examinationAudits = new List<AuditEntry<Examination>>();

                foreach (var examination in examinations)
                {
                    var previousScore = examination.UrgencyScore;

                    examination.UpdateCaseUrgencyScore();

                    if (examination.UrgencyScore != previousScore)
                    {
                        examination.LastModifiedBy = "UpdateExaminationService";
                        examination.ModifiedAt = DateTimeOffset.UtcNow;

                        examinationAudits.Add(new AuditEntry<Examination>(examination));
                    }
                }

                Logger.Log(LogLevel.Information, $"Reviewed: {examinations.Count} open examinations, updated: {examinationAudits.Count}.");

                var updateResponse = await examinationStore.UpdateRangeAsync(examinations, null, cancellationToken);

                // Assume only successful responses come back with a charge that was can use.
                var totalRequestChargeForUpdates = updateResponse.SuccessfulEntities.Sum(e => e.ResourceResponse.RequestCharge);

                var createAuditResponse = await examinationAuditStore.AddRangeAsync(examinationAudits, null, cancellationToken);

                var totalRequestChargeForAudit =
                    createAuditResponse.SuccessfulEntities.Sum(e => e.ResourceResponse.RequestCharge);

                Logger.Log(LogLevel.Information, $"Consumed total RU: {initialQueryCharge+totalRequestChargeForUpdates + totalRequestChargeForAudit} Query: {initialQueryCharge} Updates: {totalRequestChargeForUpdates} Audits:{totalRequestChargeForAudit}");
            }
        }
    }
}
