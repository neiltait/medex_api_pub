using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    public interface IExaminationPersistence
    {
        Task<bool> SaveExaminationAsync(Examination examination);
        Task<Examination> GetExaminationAsync(string Id);
        Task<IEnumerable<Examination>> GetExaminationsAsync();
    }
}
