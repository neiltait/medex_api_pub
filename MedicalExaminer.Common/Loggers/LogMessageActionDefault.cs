using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Loggers
{
    /// <summary>
    ///     Log message for before controller action has executed
    /// </summary>
    public class LogMessageActionDefault
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="userAuthenticationType">user authentication type</param>
        /// <param name="userIsAuthenticated">user authentication status</param>
        /// <param name="controllerName">name of controller called</param>
        /// <param name="controllerMethod">method of caller called</param>
        /// <param name="parameters">list of parameters passed to method</param>
        /// <param name="remoteIP">IP address of client</param>
        /// <param name="timestamp">timestamp when method called</param>
        public LogMessageActionDefault(
            string userName,
            string userAuthenticationType,
            bool userIsAuthenticated,
            string controllerName,
            string controllerMethod,
            IList<string> parameters,
            string remoteIP,
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

        /// <summary>
        ///     name of user
        /// </summary>
        public string UserName { get; }

        /// <summary>
        ///     authentication type of user
        /// </summary>
        public string UserAuthenticationType { get; }

        /// <summary>
        ///     user authentication status
        /// </summary>
        public bool UserIsAuthenticated { get; }

        /// <summary>
        ///     name of controller
        /// </summary>
        public string ControllerName { get; }

        /// <summary>
        ///     method called
        /// </summary>
        public string ControllerMethod { get; }

        /// <summary>
        ///     List of all parameters (as strings)
        /// </summary>
        public IList<string> Parameters { get; }

        /// <summary>
        ///     IP of user's machine
        /// </summary>
        public string RemoteIP { get; }

        /// <summary>
        ///     Time of call
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        ///     ToString()
        /// </summary>
        /// <returns>string</returns>
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