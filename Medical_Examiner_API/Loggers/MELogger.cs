using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Medical_Examiner_API.Persistence;

namespace Medical_Examiner_API.Loggers
{
    public class MELogger : IMELogger
    {
        //public DocumentClient client = null;
        //private IMELoggerPersistence _MElogger_persistence;

        private StreamWriter _streamWriter;
        //public MELogger(IMELoggerPersistence MElogger_persistence)
        public MELogger()
        {
            var projectFolder = Environment.CurrentDirectory;
            _streamWriter = new StreamWriter(projectFolder + "MElogging.txt");

            //_MElogger_persistence = MElogger_persistence;

        }

        public async void Log(string userName, string userAuthenticationType, bool userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
           var logEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerMethod, parameters, remoteIP, timeStamp);
            _streamWriter.WriteLine(logEntry.ToString());
            _streamWriter.Flush();

            //await _MElogger_persistence.SaveLogEntryAsync(logEntry);


        }


       
    }
}
