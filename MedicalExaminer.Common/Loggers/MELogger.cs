using System;
using System.Collections.Generic;

//using Microsoft.Azure.Documents;
//using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Loggers
{
    /// <summary>
    /// Construct log objects and submit to logging destination
    /// </summary>
    public class MELogger : IMELogger
    {
        private readonly IMeLoggerPersistence _mEloggerPersistence;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mEloggerPersistence">persistence object that writes to logging destination</param>
        public MELogger(IMeLoggerPersistence mEloggerPersistence)
        {
            _mEloggerPersistence = mEloggerPersistence;
        }

        /// <inheritdoc />
        public async void Log(string userName, string userAuthenticationType, bool userIsAuthenticated,
            string controllerName, string controllerMethod, IList<string> parameters, string remoteIP,
            DateTime timeStamp)
        {
            var logEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated,
                controllerName, controllerMethod, parameters, remoteIP, timeStamp);
            await _mEloggerPersistence.SaveLogEntryAsync(logEntry);
        }
    }
}