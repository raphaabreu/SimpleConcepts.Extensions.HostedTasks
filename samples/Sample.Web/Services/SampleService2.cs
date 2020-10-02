using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sample.Web.Services
{
    public class SampleService2
    {
        private readonly ILogger<SampleService2> _logger;

        public SampleService2(ILogger<SampleService2> logger)
        {
            _logger = logger;
        }

        public Task CheckFirstQueueAsync()
        {
            _logger.LogInformation($"{nameof(SampleService2)} checking first queue...");

            return Task.CompletedTask;

        }

        public Task CheckSecondQueueAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SampleService2)} checking second queue...");

            return Task.CompletedTask;

        }

        public Task CheckThirdQueueAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SampleService2)} checking third queue...");

            return Task.CompletedTask;

        }
    }
}
