using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SimpleConcepts.Hosting.BackgroundAction;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class RecurringTaskExtensions
    {
        public static IServiceCollection AddRecurringTask(this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task> taskFactory)
        {
            return services.AddRecurringTask(taskFactory, opt => { });
        }

        public static IServiceCollection AddRecurringTask(this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task> taskFactory,
            Action<RecurringTaskOptions> configureOptions)
        {
            var options = new RecurringTaskOptions();
            configureOptions(options);

            services.AddSingleton<IHostedService>(provider =>
                ActivatorUtilities.CreateInstance<RecurringTaskHostedService>(provider, taskFactory));

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
            var options = new RecurringTaskOptions();
            configureOptions(options);

            services.AddSingleton<IHostedService>(provider =>
                ActivatorUtilities.CreateInstance<RecurringTaskHostedService<T>>(provider, taskFactory));

            return services;
        }
    }
}
