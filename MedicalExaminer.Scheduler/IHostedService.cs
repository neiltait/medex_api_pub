using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedicalExaminer.Scheduler
{
    /// <summary>
    /// Hosted Service Interface.
    /// </summary>
    public interface IHostedService
    {
        /// <summary>
        /// Start Async
        /// </summary>
        /// <remarks>Called when the host wants to start the service.</remarks>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Task"/>.</returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stop Async
        /// </summary>
        /// <remarks>Called when the host wants to stop the service.</remarks>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Task"/>.</returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
