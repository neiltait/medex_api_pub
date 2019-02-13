using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Loggers
{
    public class MELogger : IMELogger
    {
        public void Log(string userName, string userAuthenticationType, string userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
            var djpToDo = 1;
        }
    }
}
