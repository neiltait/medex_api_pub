using System;
using System.Collections.Generic;

namespace MedicalExaminer.Common.Loggers
{
    /// <summary>
    ///     Mocker for MELogger class to be used in unit tests
    /// </summary>
    public class MELoggerMocker : IMELogger
    {
        /// <summary>
        ///     LogEntry class, exposed so that it's contents can be examined externally
        /// </summary>
        public LogMessageActionDefault LogEntry { get; private set; }

        /// <inheritdoc />
        public void Log(
            string userName,
            string userAuthenticationType,
            bool userIsAuthenticated,
            string controllerName,
            string controllerMethod,
            IList<string> parameters,
            string remoteIP,
            DateTime timeStamp)
        {
            LogEntry = new LogMessageActionDefault(
                userName,
                userAuthenticationType,
                userIsAuthenticated,
                controllerName,
                controllerMethod,
                parameters,
                remoteIP,
                timeStamp);
        }
    }
}