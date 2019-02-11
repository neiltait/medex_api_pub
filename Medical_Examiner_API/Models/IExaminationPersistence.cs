using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models
{
    public interface IExaminationPersistence
    {
        Task<bool> SaveExaminationAsync(Examination examination);
        Task<Examination> GetExaminationAsync(string Id);
        Task<IEnumerable<Examination>> GetExaminationsAsync();
    }
}
