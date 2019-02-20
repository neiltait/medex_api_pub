using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Loggers
{
    /// <summary>
    /// Mocker for MELogger class to be used in unit tests
    /// </summary>
    public class MELoggerMocker : IMELogger
    {
        /// <summary>
        /// LogEntry class, exposed so that it's contents can be examined externally
        /// </summary>
        public LogMessageActionDefault LogEntry { get; private set; }

        /// <summary>
        /// Set LogEntry property based on parameters
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="userAuthenticationType">user authentication type</param>
        /// <param name="userIsAuthenticated">user authentication status</param>
        /// <param name="controllerName">name of controller called</param>
        /// <param name="controllerMethod">method of caller called</param>
        /// <param name="parameters">list of parameters passed to method</param>
        /// <param name="remoteIP">IP address of client</param>
        /// <param name="timeStamp">timestamp when method called</param>
        public void Log(string userName, string userAuthenticationType, bool userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp)
        {
            LogEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated, controllerName, controllerMethod, parameters, remoteIP, timeStamp);
        }
    }
}
