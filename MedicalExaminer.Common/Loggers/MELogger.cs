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
            string userId,
            string userAuthenticationType,
            bool userIsAuthenticated,
            string controllerName,
            string controllerMethod,
            IDictionary<string, object> parameters,
            string remoteIP,
            DateTime timeStamp,
            double totalRus)
        {
            var logEntry = new LogMessageActionDefault(
                userId,
                userAuthenticationType,
                userIsAuthenticated,
                controllerName,
                controllerMethod,
                parameters,
                remoteIP,
                timeStamp,
                totalRus);

            await mEloggerPersistence.SaveLogEntryAsync(logEntry);
        }
    }
}