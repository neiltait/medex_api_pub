using System;
using System.Linq;
using Cosmonaut;
using Cosmonaut.Extensions;
using MedicalExaminer.Models;

namespace MedicalExaminer.Task.UpdateExaminations.Services
{
    public class UpdateExaminationsService
    {
        private readonly ICosmosStore<Examination> _examinationStore;

        public UpdateExaminationsService(ICosmosStore<Examination> examinationStore)
        {
            _examinationStore = examinationStore;
        }

        public async System.Threading.Tasks.Task Handle()
        {
            var examinations = await _examinationStore.Query().Where(e => !e.CaseCompleted).ToListAsync();

            foreach (var examination in examinations)
            {
                examination.UpdateCaseUrgencyScore();
            }

            Console.WriteLine($"Updated {examinations.Count}");
            await _examinationStore.UpdateRangeAsync(examinations);
        }
    }
}
