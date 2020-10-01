using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sample.Web.Services
{
    public class SampleService3
    {
        private readonly ILogger<SampleService3> _logger;

        public SampleService3(ILogger<SampleService3> logger)
        {
            _logger = logger;
        }

        public Task SendFinalMessageAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SampleService1)} sending final message");

            return Task.CompletedTask;
        }
    }
}
