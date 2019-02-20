using System;
using System.Collections.Generic;
using Medical_Examiner_API.Persistence;

namespace Medical_Examiner_API.Loggers
{
    public class MELogger : IMELogger
    {
        private readonly IMeLoggerPersistence _MElogger_persistence;

        public MELogger(IMeLoggerPersistence MElogger_persistence)
        {
            _MElogger_persistence = MElogger_persistence;
        }

        public async void Log(string userName, string userAuthenticationType, bool userIsAuthenticated,
            string controllerName, string controllerMethod, IList<string> parameters, string remoteIP,
            DateTime timeStamp)
        {
            var logEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated,
                controllerName, controllerMethod, parameters, remoteIP, timeStamp);
            await _MElogger_persistence.SaveLogEntryAsync(logEntry);
        }
    }
}