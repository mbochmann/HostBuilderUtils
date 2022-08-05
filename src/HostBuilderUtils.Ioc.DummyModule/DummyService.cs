using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostBuilderUtils.Ioc.DummyModule
{
    internal class DummyService : BackgroundService
    {
        private readonly TestClass _testClass;
        private readonly ILogger<DummyService> _logging;

        public DummyService(TestClass testClass, ILogger<DummyService> logging)
        {
            _testClass = testClass;
            _logging = logging;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logging.LogInformation("Test");
            Console.WriteLine("Dummy Service executed");
            await Task.CompletedTask;
        }
    }
}