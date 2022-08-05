using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;
using HostBuilderUtils.PreBuildServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Unity.Microsoft.DependencyInjection;

namespace HostBuilderUtils.PreBuildServiceProvider.Test
{
    public class AddPreBuildServiceProviderTests
    {
        private static IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder().AddPreBuildServiceProvider();
        private static IHostBuilder CreateHostBuilderWithUnity() => Host.CreateDefaultBuilder().UseUnityServiceProvider().AddPreBuildServiceProvider();

        private static async Task RunHostBuilderWithArg(IHostBuilder hostBuilder)
        {
            Assert.NotNull(hostBuilder);
            IHost host = hostBuilder.Build();
            Assert.NotNull(host);
            Exception? exception = await Record.ExceptionAsync(async () =>
            {
                await host.StartAsync();
                await host.StopAsync();
                await host.WaitForShutdownAsync();
            });
            Assert.Null(exception);
            await Task.CompletedTask;
        }

        [Fact]
        public async void RunHostBuilder()
        {
            await RunHostBuilderWithArg(CreateHostBuilder());
        }

        [Fact]
        public async void ServiceProviderNotNull()
        {
            bool calledFunction = false;
            var hostBuilder = CreateHostBuilder()
                .ConfigureServices((ctx, services, serviceProvider) =>
                {
                    calledFunction = true;
                    Assert.NotNull(serviceProvider);
                });
            await RunHostBuilderWithArg(hostBuilder);
            Assert.True(calledFunction);
        }

        [Fact]
        public async void RunHostBuilderUnity()
        {
            await RunHostBuilderWithArg(CreateHostBuilderWithUnity());
        }

        [Fact]
        public async void ServiceProviderNotNullUnity()
        {
            bool calledFunction = false;
            var hostBuilder = CreateHostBuilderWithUnity()
                .ConfigureServices((ctx, services, serviceProvider) =>
                {
                    calledFunction = true;
                    Assert.NotNull(serviceProvider);
                });
            await RunHostBuilderWithArg(hostBuilder);
            Assert.True(calledFunction);
        }
    }
}