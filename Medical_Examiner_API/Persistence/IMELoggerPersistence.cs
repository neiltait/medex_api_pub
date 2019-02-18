using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Loggers;

namespace Medical_Examiner_API.Persistence
{
    public interface IMELoggerPersistence
    {
        Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry);
    }
}
