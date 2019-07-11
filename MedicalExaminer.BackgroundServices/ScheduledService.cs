using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.BackgroundServices
{
    /// <summary>
    /// Scheduled Service.
    /// </summary>
    /// <inheritdoc cref="IHostedService"/>
    /// <inheritdoc cref="IDisposable"/>
    public abstract class ScheduledService : IHostedService, IDisposable
    {
        private readonly ILogger<ScheduledService> _logger;

        /// <summary>
        /// Configuration for scheduling
        /// </summary>
        private readonly IScheduledServiceConfiguration _configuration;

        /// <summary>
        /// The scheduler.
        /// </summary>
        private readonly IScheduler _scheduler;

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
        /// Logger
        /// </summary>
        public ILogger Logger => _logger;

        /// <summary>
        /// Initialise a new instance of <see cref="ScheduledService"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="configuration">Scheduled Service Configuration.</param>
        /// <param name="scheduler">The scheduler.</param>
        protected ScheduledService(ILogger<ScheduledService> logger, IScheduledServiceConfiguration configuration, IScheduler scheduler)
        {
            _logger = logger;
            _configuration = configuration;
            _scheduler = scheduler;
            _lastExecuted = _scheduler.LastExecuted;
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

            await Task.WhenAny(_task, _scheduler.Delay(-1, cancellationToken));

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
                var dateTimeNow = _scheduler.UtcNow;

                if (_configuration.CanExecute(dateTimeNow, _lastExecuted))
                {
                    _lastExecuted = dateTimeNow;

                    try
                    {
                        await ExecuteAsync(cancellationToken);
                    }
                    catch(Exception e)
                    {
                        _logger.LogCritical(e, "Exception occurred while running scheduled service.");
                    }
                }

                await _scheduler.Delay(_configuration.SampleRate, cancellationToken);
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
