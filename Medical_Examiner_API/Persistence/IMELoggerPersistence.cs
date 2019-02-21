using System;
using System.Collections.Generic;
using System.Linq;
﻿using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    /// <summary>
    /// Interface for persistence class used by MELogger
    /// </summary>

    public interface IMeLoggerPersistence
    {
        /// <summary>
        /// Writes log entry to database
        /// </summary>
        /// <param name="logEntry">object to be logged</param>
        /// <returns>bool</returns>
        Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry);
    }
}