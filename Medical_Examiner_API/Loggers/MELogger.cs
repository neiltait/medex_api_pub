using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Persistence;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Medical_Examiner_API.Loggers
{
    public class MELogger : IMELogger
    {
        private IMELoggerPersistence _MElogger_persistence;

        public MELogger(IMELoggerPersistence MElogger_persistence)
        {
            _MElogger_persistence = MElogger_persistence;
        }

        public async void Log(string userName, string userAuthenticationType, bool userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
           var logEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerMethod, parameters, remoteIP, timeStamp);
           await _MElogger_persistence.SaveLogEntryAsync(logEntry);
        } 
    }
}
