using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.BackgroundServices
{
    /// <summary>
    /// Scheduled Service Configuration.
    /// </summary>
    public interface IScheduledServiceConfiguration
    {
        /// <summary>
        /// Sample Rate.
        /// </summary>
        /// <remarks>The time span to delay between checking whether we can execute.</remarks>
        TimeSpan SampleRate { get; }

        /// <summary>
        /// Can Execute
        /// </summary>
        /// <remarks>Determine if the scheduled service should run its execution method.</remarks>
        /// <param name="dateTime">The current Date Time</param>
        /// <param name="lastExecuted">The Date Time that the service last executed.</param>
        /// <returns>Boolean indicating whether to execute.</returns>
        bool CanExecute(DateTime dateTime, DateTime lastExecuted);
    }
}
