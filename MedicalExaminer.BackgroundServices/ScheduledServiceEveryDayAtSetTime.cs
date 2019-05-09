using System;

namespace MedicalExaminer.BackgroundServices
{
    /// <summary>
    /// Scheduled Service Every Day At Set Time.
    /// </summary>
    public class ScheduledServiceEveryDayAtSetTime : IScheduledServiceConfiguration
    {
        private readonly TimeSpan _atTime;

        /// <summary>
        /// Initialise a new instance of <see cref="ScheduledServiceEveryDayAtSetTime"/>.
        /// </summary>
        /// <param name="atTime">The Time at which every day to run.</param>
        /// <param name="sampleRate">The interval at which to check if we can execute.</param>
        public ScheduledServiceEveryDayAtSetTime(TimeSpan atTime, TimeSpan sampleRate)
        {
            _atTime = atTime;
            SampleRate = sampleRate;
        }

        /// <inheritdoc/>
        public TimeSpan SampleRate { get; }

        /// <inheritdoc/>
        public bool CanExecute(DateTime dateTime, DateTime lastExecuted)
        {
            return dateTime.TimeOfDay >= _atTime && (dateTime.Date - lastExecuted.Date).Days >= 1;
        }
    }
}
