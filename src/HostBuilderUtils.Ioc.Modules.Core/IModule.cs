using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostBuilderUtils.Ioc.Modules
{
    public interface IModule
    {
        void ConfigureServices(HostBuilderContext ctx, IServiceCollection services);
    }
}
