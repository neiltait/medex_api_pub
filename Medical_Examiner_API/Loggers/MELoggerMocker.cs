using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Loggers
{
    public class MELoggerMocker : IMELogger
    {
        public LogMessageActionDefault LogEntry { get; private set; }
        public void Log(string userName, string userAuthenticationType, bool userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
            LogEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerMethod, parameters, remoteIP, timeStamp);
        }
    }
}
