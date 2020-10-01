using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.Hosting.BackgroundAction
{
    public class LifecycleTasksOptions
    {
        public IList<Func<IServiceProvider, CancellationToken, Task>> StartupTaskFactories { get; } =
            new List<Func<IServiceProvider, CancellationToken, Task>>();
        public IList<Func<IServiceProvider, CancellationToken, Task>> ShutdownTaskFactories { get; } =
            new List<Func<IServiceProvider, CancellationToken, Task>>();
    }
}
