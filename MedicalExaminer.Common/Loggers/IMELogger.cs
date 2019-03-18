using System;
using System.Collections.Generic;

namespace MedicalExaminer.Common.Loggers
{
    /// <summary>
    ///     Interface for Logger class
    /// </summary>
    public interface IMELogger
    {
        /// <summary>
        ///     Write log entry based on parameters received
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="userAuthenticationType">user authentication type</param>
        /// <param name="userIsAuthenticated">user authentication status</param>
        /// <param name="controllerName">name of controller called</param>
        /// <param name="controllerMethod">method of caller called</param>
        /// <param name="parameters">list of parameters passed to method</param>
        /// <param name="remoteIP">IP address of client</param>
        /// <param name="timeStamp">timestamp when method called</param>
        void Log(string userName, string userAuthenticationType, bool userIsAuthenticated, string controllerName,
            string controllerMethod, IList<string> parameters, string remoteIP, DateTime timeStamp);
    }
}