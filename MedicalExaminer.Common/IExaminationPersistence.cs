using System;
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


        //DJP
        //Task<Guid> CreateExaminationAsync(Examination examinationItem);
        Task<string> CreateExaminationAsync(Examination examinationItem);
    }
}