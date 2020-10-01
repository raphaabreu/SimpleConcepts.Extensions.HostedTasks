using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Web.Services;

namespace Sample.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Services can have any lifetime, the tasks always execute within brand new service scope.
            services.AddSingleton<SampleService1>();
            services.AddScoped<SampleService2>();
            services.AddTransient<SampleService3>();

            // Register a function to be called during app startup.
            services.AddStartupTask<SampleService1>((service, token) => service.InitializeAsync(token));

            // Configure default wait times for recurring tasks.
            services.ConfigureRecurringTasks(opt =>
            {
                opt.WaitBeforeStart = TimeSpan.FromSeconds(4);
                opt.WaitAfterException = TimeSpan.FromSeconds(60);
            });

            // Register a function to be called in an infinite loop, by default the task is invoked every second.
            services.AddRecurringTask<SampleService2>((service, token) => service.CheckFirstQueueAsync(token));

            // Register another function to be called with a custom delay.
            services.AddRecurringTask<SampleService2>((service, token) => service.CheckSecondQueueAsync(token),
                opt => opt.WaitAfterCompletion = TimeSpan.FromSeconds(2.5));

            // Register another function to be called with all customizations.
            services.AddRecurringTask<SampleService2>((service, token) => service.CheckThirdQueueAsync(token),
            opt =>
            {
                opt.WaitBeforeStart = TimeSpan.FromSeconds(1);
                opt.WaitAfterCompletion = TimeSpan.FromSeconds(3.7);
                opt.WaitAfterException = TimeSpan.FromSeconds(10);
            });

            // Register a function to be called during app shutdown.
            services.AddShutdownTask<SampleService3>((service, token) => service.SendFinalMessageAsync(token));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
