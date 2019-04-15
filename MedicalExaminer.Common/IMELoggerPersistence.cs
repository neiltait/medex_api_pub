using System.Threading.Tasks;
using MedicalExaminer.Common.Loggers;

namespace MedicalExaminer.Common
{
    /// <summary>
    ///     Interface for persistence class used by MELogger
    /// </summary>
    public interface IMeLoggerPersistence
    {
        /// <summary>
        ///     Writes log entry to database
        /// </summary>
        /// <param name="logEntry">object to be logged</param>
        /// <returns>bool</returns>
        Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry);
    }
}