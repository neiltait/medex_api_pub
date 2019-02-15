using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Medical_Examiner_API.Loggers
{
    public class MELogger : IMELogger
    {
        private StreamWriter _streamWriter;
        public MELogger()
        {
            var projectFolder = Environment.CurrentDirectory;
            _streamWriter = new StreamWriter(projectFolder + "MElogging.txt");

        }

        public void Log(string userName, string userAuthenticationType, bool userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
           var logEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerMethod, parameters, remoteIP, timeStamp);
            _streamWriter.WriteLine(logEntry.ToString());
            _streamWriter.Flush();
        }


       
    }
}
