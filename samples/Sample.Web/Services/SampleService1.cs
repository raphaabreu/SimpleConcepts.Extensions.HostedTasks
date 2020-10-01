using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sample.Web.Services
{
    public class SampleService1
    {
        private readonly ILogger<SampleService1> _logger;

        public SampleService1(ILogger<SampleService1> logger)
        {
            _logger = logger;
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SampleService1)} initialization");

            return Task.CompletedTask;
        }
    }
}