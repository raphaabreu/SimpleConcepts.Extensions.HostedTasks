using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.Extensions.HostedTasks
{
    public class LifecycleTasksHostedService : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly IOptionsMonitor<LifecycleTasksOptions> _options;

        public LifecycleTasksHostedService(IServiceProvider provider, IOptionsMonitor<LifecycleTasksOptions> options)
        {
            _provider = provider;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var tasks = _options
                .CurrentValue
                .StartupTaskFactories
                .Select(f => RunScopedAsync(f, cancellationToken))
                .ToArray();

            return Task.WhenAll(tasks);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var tasks = _options
                .CurrentValue
                .ShutdownTaskFactories
                .Select(f => RunScopedAsync(f, cancellationToken))
                .ToArray();

            return Task.WhenAll(tasks);
        }

        private async Task RunScopedAsync(Func<IServiceProvider, CancellationToken, Task> taskFactory, CancellationToken cancellationToken)
        {
            using var serviceScope = _provider.CreateScope();

            await taskFactory(serviceScope.ServiceProvider, cancellationToken);
        }
    }
}
