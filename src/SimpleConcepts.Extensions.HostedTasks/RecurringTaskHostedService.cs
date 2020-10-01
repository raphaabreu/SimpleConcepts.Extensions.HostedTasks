using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.Extensions.HostedTasks
{
    public class RecurringTaskHostedService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<RecurringTaskHostedService> _logger;
        private readonly Func<IServiceProvider, CancellationToken, Task> _action;
        private readonly RecurringTaskOptions _options;

        public RecurringTaskHostedService(
            IServiceProvider provider,
            RecurringTaskOptions options,
            Func<IServiceProvider, CancellationToken, Task> taskFactory,
            ILogger<RecurringTaskHostedService> logger
        )
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _action = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(_options.WaitBeforeStart, stoppingToken);

            do
            {
                try
                {
                    using var scope = _provider.CreateScope();

                    await _action(scope.ServiceProvider, stoppingToken);

                    await Task.Delay(_options.WaitAfterCompletion, stoppingToken);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Execution failed");

                    await Task.Delay(_options.WaitAfterException, stoppingToken);
                }
            } while (!stoppingToken.IsCancellationRequested);
        }
    }

    public class RecurringTaskHostedService<T> : RecurringTaskHostedService
    {
        public RecurringTaskHostedService(
            IServiceProvider provider,
            RecurringTaskOptions options,
            Func<IServiceProvider, CancellationToken, Task> taskFactory,
            ILogger<RecurringTaskHostedService<T>> logger
        ) : base(provider, options, taskFactory, logger)
        {
        }
    }
}
