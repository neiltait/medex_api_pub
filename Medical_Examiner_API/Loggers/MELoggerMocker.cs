using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Loggers
{
    public class MELoggerMocker : IMELogger
    {
        public string Message { get; private set; }
        public void Log(string userName, string userAuthenticationType, string userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
            Message = userName + " " + userAuthenticationType + " " + userIsAuthenticated + " " + controllerName + " " + controllerMethod + " ";

            foreach (var p in parameters)
            {
                Message += p + " ";
            }

            Message += remoteIP;
        }
    }
}
