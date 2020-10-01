using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleConcepts.Hosting.BackgroundAction;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class LifecycleTasksExtensions
    {
        private static IServiceCollection AddLifecycleTasksHostedService(this IServiceCollection services)
        {
            services.Configure<LifecycleTasksOptions>(opt => { });
            services.AddHostedService<LifecycleTasksHostedService>();

            return services;
        }

        public static IServiceCollection AddStartupTask(this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task> taskFactory)
        {
            services.AddLifecycleTasksHostedService();
            services.Configure<LifecycleTasksOptions>(opt => opt.StartupTaskFactories.Add(taskFactory));

            return services;
        }

        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services,
            Func<T, CancellationToken, Task> taskFactory)
        {
            return services.AddStartupTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T>(), cancellationToken));
        }

        public static IServiceCollection AddStartupTask<T1, T2>(this IServiceCollection services,
            Func<T1, T2, CancellationToken, Task> taskFactory)
        {
            return services.AddStartupTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(), cancellationToken));
        }

        public static IServiceCollection AddStartupTask<T1, T2, T3>(this IServiceCollection services,
            Func<T1, T2, T3, CancellationToken, Task> taskFactory)
        {
            return services.AddStartupTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(),
                    provider.GetRequiredService<T3>(), cancellationToken));
        }

        public static IServiceCollection AddStartupTask<T1, T2, T3, T4>(this IServiceCollection services,
            Func<T1, T2, T3, T4, CancellationToken, Task> taskFactory)
        {
            return services.AddStartupTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(),
                    provider.GetRequiredService<T3>(), provider.GetRequiredService<T4>(), cancellationToken));
        }

        public static IServiceCollection AddStartupTask<T1, T2, T3, T4, T5>(this IServiceCollection services,
            Func<T1, T2, T3, T4, T5, CancellationToken, Task> taskFactory)
        {
            return services.AddStartupTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(),
                    provider.GetRequiredService<T3>(), provider.GetRequiredService<T4>(),
                    provider.GetRequiredService<T5>(), cancellationToken));
        }

        public static IServiceCollection AddShutdownTask(this IServiceCollection services,
            Func<IServiceProvider, CancellationToken, Task> taskFactory)
        {
            services.AddLifecycleTasksHostedService();
            services.Configure<LifecycleTasksOptions>(opt => opt.ShutdownTaskFactories.Add(taskFactory));

            return services;
        }

        public static IServiceCollection AddShutdownTask<T>(this IServiceCollection services,
            Func<T, CancellationToken, Task> taskFactory)
        {
            return services.AddShutdownTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T>(), cancellationToken));
        }

        public static IServiceCollection AddShutdownTask<T1, T2>(this IServiceCollection services,
            Func<T1, T2, CancellationToken, Task> taskFactory)
        {
            return services.AddShutdownTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(), cancellationToken));
        }

        public static IServiceCollection AddShutdownTask<T1, T2, T3>(this IServiceCollection services,
            Func<T1, T2, T3, CancellationToken, Task> taskFactory)
        {
            return services.AddShutdownTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(),
                    provider.GetRequiredService<T3>(), cancellationToken));
        }

        public static IServiceCollection AddShutdownTask<T1, T2, T3, T4>(this IServiceCollection services,
            Func<T1, T2, T3, T4, CancellationToken, Task> taskFactory)
        {
            return services.AddShutdownTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(),
                    provider.GetRequiredService<T3>(), provider.GetRequiredService<T4>(), cancellationToken));
        }

        public static IServiceCollection AddShutdownTask<T1, T2, T3, T4, T5>(this IServiceCollection services,
            Func<T1, T2, T3, T4, T5, CancellationToken, Task> taskFactory)
        {
            return services.AddShutdownTask((provider, cancellationToken) =>
                taskFactory(provider.GetRequiredService<T1>(), provider.GetRequiredService<T2>(),
                    provider.GetRequiredService<T3>(), provider.GetRequiredService<T4>(),
                    provider.GetRequiredService<T5>(), cancellationToken));
        }
    }
}
