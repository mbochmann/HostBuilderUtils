// See https://aka.ms/new-console-template for more information
using HostBuilderUtils.Ioc.Modules;
using HostBuilderUtils.PreBuildServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unity.Microsoft.DependencyInjection;

Console.WriteLine("Hello, World!");
var host = CreateHostBuilder(args).Build();
host.Run();

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host
        .CreateDefaultBuilder(args)
        .UseUnityServiceProvider()
        .ConfigureServices((ctx, services) =>
        {
            services.AddLogging();
        })
        .AddPreBuildServiceProvider()
        .ConfigureServices((ctx, services, sp) =>
        {
            if (sp.GetService<ILogger<Program>>() is ILogger logger)
                logger.LogInformation("Configure services");
        })
        .UseModules();
}