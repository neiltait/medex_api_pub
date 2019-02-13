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

        public void Log(string userName, string userAuthenticationType, string userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
            var message = timeStamp.ToLongDateString() + " "+ timeStamp.ToShortTimeString() + " " + userName + " " + userAuthenticationType + " " + userIsAuthenticated + " " + controllerName + " " + controllerMethod + " ";

            foreach (var p in parameters)
            {
                message += p + " ";
            }

            message += remoteIP;
            _streamWriter.WriteLine(message);
            _streamWriter.Flush();
        }


       
    }
}
