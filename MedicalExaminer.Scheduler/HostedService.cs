using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedicalExaminer.Scheduler
{
    public abstract class HostedService : IHostedService
    {
        private Task _task;

        private CancellationTokenSource _cancellationTokenSource;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _task = ExecuteEveryAsync(_cancellationTokenSource.Token);

            return _task.IsCompleted ? _task : Task.CompletedTask;
        }

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

        private async Task ExecuteEveryAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteAsync(cancellationToken);
                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
            }
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
