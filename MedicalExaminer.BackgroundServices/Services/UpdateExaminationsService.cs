using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using MedicalExaminer.Models;

namespace MedicalExaminer.BackgroundServices.Services
{
    /// <summary>
    /// Update Examinations Service.
    /// </summary>
    public class UpdateExaminationsService : ScheduledService
    {
        private readonly ICosmosStore<Examination> _examinationStore;

        /// <summary>
        /// Initialise a new instance of <see cref="UpdateExaminationsService"/>.
        /// </summary>
        /// <param name="configuration">Scheduled service configuration.</param>
        /// <param name="examinationStore">Examination Store.</param>
        public UpdateExaminationsService(IScheduledServiceConfiguration configuration, ICosmosStore<Examination> examinationStore)
            : base(configuration)
        {
            _examinationStore = examinationStore;
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Running!!");

            return Task.CompletedTask;

            /*var examinations = await _examinationStore.Query().Where(e => !e.CaseCompleted).ToListAsync();

            foreach (var examination in examinations)
            {
                examination.UpdateCaseUrgencyScore();
            }

            Console.WriteLine($"Updated {examinations.Count}");
            await _examinationStore.UpdateRangeAsync(examinations);*/
        }
    }
}
