using System;

namespace MedicalExaminer.Common.Settings
{
    /// <summary>
    /// Background Services Settings.
    /// </summary>
    public class BackgroundServicesSettings
    {
        /// <summary>
        /// Gets or sets the time to run each day.
        /// </summary>
        public TimeSpan TimeToRunEachDay { get; set; }

        /// <summary>
        /// Gets or sets the sample rate at which to query if its time to run.
        /// </summary>
        /// <remarks>Hourly should be enough, but if the system is started at 00:59 and the time to run is set at 02:00; after an hour (the time is 01:59) it wont be time to run, so the next time it will run will be 02:59</remarks>
        public TimeSpan SampleRate { get; set; }
    }
}
