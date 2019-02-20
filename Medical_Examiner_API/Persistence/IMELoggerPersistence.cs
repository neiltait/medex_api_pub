using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;

namespace Medical_Examiner_API.Persistence
{
    public interface IMeLoggerPersistence
    {
        Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry);
    }
}