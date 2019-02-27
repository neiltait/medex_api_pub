using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common
{
    public interface IExaminationPersistence
    {
        Task<bool> SaveExaminationAsync(Examination examination);
        Task<Examination> GetExaminationAsync(string examinationId);
        Task<IEnumerable<Examination>> GetExaminationsAsync();
    }
}