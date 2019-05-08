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
        public ScheduledServiceEveryDayAtSetTime(TimeSpan atTime)
        {
            _atTime = atTime;
        }

        // TODO: Work this out as a multiple of the at time.
        /// <inheritdoc/>
        public TimeSpan SampleRate => TimeSpan.FromSeconds(30);

        /// <inheritdoc/>
        public bool CanExecute(DateTime dateTime, DateTime lastExecuted)
        {
            return dateTime.TimeOfDay > _atTime && (dateTime.Date - lastExecuted.Date).Days >= 1;
        }
    }
}
