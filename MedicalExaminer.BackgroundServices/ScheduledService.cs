using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MedicalExaminer.BackgroundServices
{
    /// <summary>
    /// Scheduled Service.
    /// </summary>
    /// <inheritdoc cref="IHostedService"/>
    /// <inheritdoc cref="IDisposable"/>
    public abstract class ScheduledService : IHostedService, IDisposable
    {
        /// <summary>
        /// Configuration for scheduling
        /// </summary>
        private readonly IScheduledServiceConfiguration _configuration;

        /// <summary>
        /// When did the service last run its execute.
        /// </summary>
        private DateTime _lastExecuted;

        /// <summary>
        /// The outermost task that is running the scheduled service.
        /// </summary>
        private Task _task;

        /// <summary>
        /// A source for generating cancellation tokens.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Initialise a new instance of <see cref="ScheduledService"/>.
        /// </summary>
        /// <param name="configuration">Scheduled Service Configuration.</param>
        protected ScheduledService(IScheduledServiceConfiguration configuration)
        {
            _configuration = configuration;
            _lastExecuted = DateTime.MinValue;
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _task = ExecuteScheduledAsync(_cancellationTokenSource.Token);

            return _task.IsCompleted ? _task : Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_task == null)
            {
                return;
            }

            _cancellationTokenSource.Cancel();

            await Task.WhenAny(_task, Task.Delay(-1, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// Execute Async.
        /// </summary>
        /// <param name="cancellationToken">The Cancellation Token</param>
        /// <returns><see cref="Task"/>.</returns>
        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        private async Task ExecuteScheduledAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var dateTimeNow = DateTime.UtcNow;

                if (_configuration.CanExecute(dateTimeNow, _lastExecuted))
                {
                    _lastExecuted = dateTimeNow;

                    await ExecuteAsync(cancellationToken);
                }

                await Task.Delay(_configuration.SampleRate, cancellationToken);
            }
        }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}
