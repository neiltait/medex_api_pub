using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedicalExaminer.BackgroundServices
{
    /// <summary>
    /// Scheduler.
    /// </summary>
    /// <seealso cref="MedicalExaminer.BackgroundServices.IScheduler" />
    public class Scheduler : IScheduler
    {
        /// <inheritdoc/>
        public DateTime LastExecuted => DateTime.MinValue;

        /// <inheritdoc/>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <inheritdoc/>
        public Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            return Task.Delay(timeSpan, cancellationToken);
        }

        /// <inheritdoc/>
        public Task Delay(int milliseconds, CancellationToken cancellationToken)
        {
            return Task.Delay(milliseconds, cancellationToken);
        }
    }
}
