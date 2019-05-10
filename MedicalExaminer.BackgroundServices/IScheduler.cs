using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedicalExaminer.BackgroundServices
{
    /// <summary>
    /// Scheduler Interface.
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Gets the last executed.
        /// </summary>
        DateTime LastExecuted { get; }

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Delays the specified time span.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken);

        /// <summary>
        /// Delays the specified milliseconds.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task Delay(int milliseconds, CancellationToken cancellationToken);
    }
}
