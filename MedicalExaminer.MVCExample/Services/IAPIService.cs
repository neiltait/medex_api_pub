using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.MVCExample.Services
{
    public interface IAPIService
    {
        Task<IEnumerable<object>> GetValues(string token);
    }
}
