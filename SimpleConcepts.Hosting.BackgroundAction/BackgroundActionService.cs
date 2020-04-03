using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.Hosting.BackgroundAction
{
    public class BackgroundActionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundActionService> _logger;
        private readonly Func<IServiceProvider, CancellationToken, Task> _action;
        private readonly BackgroundActionOptions _options;

        public BackgroundActionService(
            IServiceProvider serviceProvider,
            BackgroundActionOptions options,
            Func<IServiceProvider, CancellationToken, Task> action,
            ILogger<BackgroundActionService> logger
        )
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(_options.WaitBeforeStart, stoppingToken);

            do
            {
                try
                {
                    await Task.Delay(_options.WaitBeforeExecution, stoppingToken);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        await _action(scope.ServiceProvider, stoppingToken);
                    }
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

    public class BackgroundActionService<T> : BackgroundActionService
    {
        public BackgroundActionService(
            IServiceProvider serviceProvider,
            BackgroundActionOptions options,
            Func<IServiceProvider, CancellationToken, Task> action,
            ILogger<BackgroundActionService<T>> logger
        ) : base(serviceProvider, options, action, logger)
        {
        }
    }
}
