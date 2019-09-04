using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.MELogger
{
    /// <summary>
    /// Create ME Logger Service.
    /// </summary>
    public class CreateMELoggerService : QueryHandler<CreateMELoggerQuery, LogMessageActionDefault>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CreateMELoggerService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">User Connection Settings.</param>
        public CreateMELoggerService(IDatabaseAccess databaseAccess, IMELoggerConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<LogMessageActionDefault> Handle(CreateMELoggerQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = await CreateItemAsync(param.LogEntry);
            return result;
        }
    }
}