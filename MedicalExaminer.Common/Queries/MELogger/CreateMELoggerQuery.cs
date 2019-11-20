using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.MELogger
{
    /// <inheritdoc />
    public class CreateMELoggerQuery : IQuery<LogMessageActionDefault>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CreateMELoggerQuery"/>.
        /// </summary>
        /// <param name="logEntry"></param>
        public CreateMELoggerQuery(LogMessageActionDefault logEntry)
        {
            LogEntry = logEntry;
        }

        /// <summary>
        /// Log Entry to create.
        /// </summary>
        public LogMessageActionDefault LogEntry { get; }
    }
}