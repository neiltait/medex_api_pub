using System;
using System.Collections.Generic;

namespace MedicalExaminer.Common.Loggers
{
    /// <summary>
    ///     Construct log objects and submit to logging destination
    /// </summary>
    public class MELogger : IMELogger
    {
        private readonly IMeLoggerPersistence mEloggerPersistence;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MELogger" /> class.
        /// </summary>
        /// <param name="mEloggerPersistence">persistence object that writes to logging destination</param>
        public MELogger(IMeLoggerPersistence mEloggerPersistence)
        {
            this.mEloggerPersistence = mEloggerPersistence;
        }

        /// <inheritdoc />
        public async void Log(
            string userName,
            string userAuthenticationType,
            bool userIsAuthenticated,
            string controllerName,
            string controllerMethod,
            IList<string> parameters,
            string remoteIP,
            DateTime timeStamp)
        {
            var logEntry = new LogMessageActionDefault(
                userName,
                userAuthenticationType,
                userIsAuthenticated,
                controllerName,
                controllerMethod,
                parameters,
                remoteIP,
                timeStamp);

            await mEloggerPersistence.SaveLogEntryAsync(logEntry);
        }
    }
}