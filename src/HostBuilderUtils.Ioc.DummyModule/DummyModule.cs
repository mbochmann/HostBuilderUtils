using HostBuilderUtils.Ioc.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostBuilderUtils.Ioc.DummyModule
{
    public class DummyModule : IModule
    {
        public void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services
               .AddHostedService<DummyService>();
        }
    }
}