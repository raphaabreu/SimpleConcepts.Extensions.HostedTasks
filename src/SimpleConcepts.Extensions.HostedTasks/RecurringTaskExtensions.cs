using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SimpleConcepts.Extensions.HostedTasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class RecurringTaskExtensions
    {
        public static IServiceCollection AddRecurringTask(this IServiceCollection services,
            Func<IServiceProvider, Task> taskFactory)
        {
            return services.AddRecurringTask((provider, token) => taskFactory(provider), opt => { });
        }

        public static IServiceCollection AddRecurringTask(this IServiceCollection services,
            Func<IServiceProvider, Task> taskFactory,
            Action<RecurringTaskOptions> configureOptions)
        {
            return services.AddRecurringTask((provider, token) => taskFactory(provider), configureOptions);
        }
        public static IServiceCollection AddRecurringTask<T>(this IServiceCollection services,
            Func<T, Task> taskFactory)
        {
            return services.AddRecurringTask<T>(taskFactory, opt => { });
        }

        public static IServiceCollection AddRecurringTask<T>(this IServiceCollection services,
            Func<T, Task> taskFactory,
            Action<RecurringTaskOptions> configureOptions)
        {
            return services.AddRecurringTask<T>((t, token) => taskFactory(t), configureOptions);
        }

        public static IServiceCollection AddRecurringTask(this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task> taskFactory)
        {
            return services.AddRecurringTask(taskFactory, opt => { });
        }

        public static IServiceCollection AddRecurringTask(this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task> taskFactory,
            Action<RecurringTaskOptions> configureOptions)
        {
            services.AddTransient<RecurringTaskHostedService>();
            services.AddSingleton<IHostedService>(provider =>
                ActivatorUtilities.CreateInstance<RecurringTaskHostedService>(provider, provider.MergeOptions(configureOptions), taskFactory));

            return services;
        }

        public static IServiceCollection AddRecurringTask<T>(this IServiceCollection services,
            Func<T, CancellationToken, Task> taskFactory)
        {
            return services.AddRecurringTask<T>(taskFactory, opt => { });
        }

        public static IServiceCollection AddRecurringTask<T>(this IServiceCollection services,
            Func<T, CancellationToken, Task> taskFactory,
            Action<RecurringTaskOptions> configureOptions)
        {
            services.Add(ServiceDescriptor.Transient(typeof(RecurringTaskHostedService<>), typeof(RecurringTaskHostedService<>)));
            services.AddSingleton<IHostedService>(provider =>
                ActivatorUtilities.CreateInstance<RecurringTaskHostedService<T>>(provider, provider.MergeOptions(configureOptions),
                    new Func<IServiceProvider, CancellationToken, Task>((scopedProvider, cancellationToken) =>
                        taskFactory(scopedProvider.GetRequiredService<T>(), cancellationToken))));

            return services;
        }

        public static IServiceCollection ConfigureRecurringTasks(this IServiceCollection services,
            Action<RecurringTaskOptions> configureOptions)
        {
            services.AddSingleton(configureOptions);

            return services;
        }

        private static RecurringTaskOptions MergeOptions(this IServiceProvider provider,
            Action<RecurringTaskOptions> configureOptions)
        {
            var configurators = provider.GetRequiredService<IEnumerable<Action<RecurringTaskOptions>>>();
            var opts = new RecurringTaskOptions();

            foreach (var configurator in configurators)
            {
                configurator(opts);
            }
            configureOptions(opts);

            return opts;
        }
    }
}
