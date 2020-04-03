using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleConcepts.Hosting.BackgroundAction;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackgroundAction<TService>(this IServiceCollection services, Func<TService, CancellationToken, Task> action)
        {
            services.AddSingleton<IHostedService>(rootProvider => new BackgroundActionService<TService>(rootProvider,
                new BackgroundActionOptions(),
                (scopedProvider, token) => action(scopedProvider.GetRequiredService<TService>(), token),
                rootProvider.GetRequiredService<ILogger<BackgroundActionService<TService>>>()));

            return services;
        }

        public static IServiceCollection AddBackgroundAction<TService>(this IServiceCollection services, Action<BackgroundActionOptions> configureOptions, Func<TService, CancellationToken, Task> action)
        {
            var options = new BackgroundActionOptions();

            configureOptions(options);

            services.AddSingleton<IHostedService>(rootProvider => new BackgroundActionService<TService>(rootProvider,
                options,
                (scopedProvider, token) => action(scopedProvider.GetRequiredService<TService>(), token),
                rootProvider.GetRequiredService<ILogger<BackgroundActionService<TService>>>()));

            return services;
        }

        public static IServiceCollection AddBackgroundAction(this IServiceCollection services, Func<IServiceProvider, CancellationToken, Task> action)
        {
            services.AddSingleton<IHostedService>(provider => new BackgroundActionService(provider,
                new BackgroundActionOptions(),
                action,
                provider.GetRequiredService<ILogger<BackgroundActionService>>()));

            return services;
        }

        public static IServiceCollection AddBackgroundAction(this IServiceCollection services, Action<BackgroundActionOptions> configureOptions, Func<IServiceProvider, CancellationToken, Task> action)
        {
            var options = new BackgroundActionOptions();

            configureOptions(options);

            services.AddSingleton<IHostedService>(provider => new BackgroundActionService(provider,
                options,
                action,
                provider.GetRequiredService<ILogger<BackgroundActionService>>()));

            return services;
        }
    }
}
