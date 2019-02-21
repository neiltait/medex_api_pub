using System;
using System.Collections.Generic;
using System.Text;

namespace Medical_Examiner_API.Loggers
{
    public class LogMessageActionDefault
    {
        public LogMessageActionDefault(string userName, string userAuthenticationType, bool userIsAuthenticated,
            string controllerName, string controllerMethod, IList<string> parameters, string remoteIP,
            DateTime timestamp)
        {
            UserName = userName;
            UserAuthenticationType = userAuthenticationType;
            UserIsAuthenticated = userIsAuthenticated;
            ControllerName = controllerName;
            ControllerMethod = controllerMethod;
            Parameters = parameters;
            RemoteIP = remoteIP;
            TimeStamp = timestamp;
        }

        public string UserName { get; }
        public string UserAuthenticationType { get; }
        public bool UserIsAuthenticated { get; }
        public string ControllerName { get; }
        public string ControllerMethod { get; }
        public IList<string> Parameters { get; }
        public string RemoteIP { get; }
        public DateTime TimeStamp { get; }

        public override string ToString()
        {
            var contents = new StringBuilder();
            contents.Append(UserName + " " + UserAuthenticationType + " " + UserIsAuthenticated + " " + ControllerName +
                            " " + ControllerMethod + " ");

            foreach (var p in Parameters) contents.Append(p + " ");

            contents.Append(RemoteIP + " ");
            contents.Append(TimeStamp.ToLongDateString() + "_" + TimeStamp.ToLongTimeString());

            return contents.ToString();
        }
    }
}