# SimpleConcepts.Extensions.HostedTasks

This package provides extensions to define tasks that should be run at service startup/shutdown or in a recurring pattern.

Check the included project in `samples` to see a general purpose implementation.

## Installation

With package Manager:
```
Install-Package SimpleConcepts.Extensions.HostedTasks
```

With .NET CLI:
```
dotnet add package SimpleConcepts.Extensions.HostedTasks
```

## Startup and shutdown tasks

The .net provide a native `IHostApplicationLifetime` that can be used to register callbacks that will be invoked when the application starts or stops.

This is a good approach when you need to register some action dynamically after the host has already been fully configurated because the only way to access `IHostApplicationLifetime` is through dependency injection and that only becomes available once all services have been configured.

For libraries to register actions to take place on these events it is usually necessary to write an `IHostedService` to hook into the startup/shutdown process. This is simple but has some disadvantages, the main one is that hosted services are not intialized in parallel, so if you need to intialize connections to different database providers for instance, your overall startup time will be longer.

This package provides an alternative approach: all startup/shutdown tasks can be specified at configuration time before the service provider is built and all tasks run in parallel once the host actually starts.

To add a startup task:

```csharp
// Register a function to be called during app startup.
services.AddStartupTask<IClusterService>((service, cancellationToken) => service.DiscoverNodesAsync(cancellationToken));
```

To add a shutdown task:

```csharp
// Register a function to be called during app shutdown.
services.AddShutdownTask<IClusterService>((service, cancellationToken) => service.DisconnectAsync(cancellationToken));
```

Both methods have overloads that allow easy injection of multiple services if needed.

```csharp
services.AddStartupTask<IConfiguration, IClusterService, ILogger>(async (config, cluster, logger, token) =>
{
    cluster.DiscoveryUrl = config["ClusterUrl"];
    try {
        await cluster.ConnectAsync(token);
    } catch (Exception ex) {
        logger.LogError(ex, "Failed to connect to cluster.");
    }
});
```

## Recurring tasks

Another approach very common in some scenarios is to have to call a function on a particular service to check for updates or do some sort of recurring action.

To achieve this, you could again write a `IHostedService` that runs a loop and executes the desired action and this package removes the need for such boilerplate code.

To setup a recurring task, simply call:

```csharp
// Add recurring task with default intervals.
services.AddRecurringTask<IQueueService>((service, cancellationToken) => service.CheckQueueAsync(cancellationToken));
```

You can optionally configure how often you want the function to be invoked:

```csharp
// Add recurring task with custom intervals.
services.AddRecurringTask<IQueueService>((service, cancellationToken) => service.CheckQueueAsync(cancellationToken), opt =>
{
    opt.WaitBeforeStart = TimeSpan.FromSeconds(1);
    opt.WaitAfterCompletion = TimeSpan.FromSeconds(3.7);
    opt.WaitAfterException = TimeSpan.FromSeconds(10);
});
```

If you have multiple recurring tasks to add, it is also possible to configure custom base defaults:

```csharp
// Configure default intervals.
services.ConfigureRecurringTasks(opt =>
{
    opt.WaitBeforeStart = TimeSpan.FromSeconds(4);
    opt.WaitAfterException = TimeSpan.FromSeconds(60);
});
```
