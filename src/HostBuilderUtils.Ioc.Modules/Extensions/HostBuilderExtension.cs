using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostBuilderUtils.Ioc.Modules
{
    public static class HostBuilderExtension
    {
        public static IHostBuilder UseModules(this IHostBuilder hostBuilder, Action<IModuleManagerConfiguration>? options = null)
            => hostBuilder.ConfigureServices((ctx, services) => services.UseModules(options, ctx));
        public static IHostBuilder UseModules(this IHostBuilder hostBuilder, IModuleCatalog moduleCatalog, Action<IModuleManagerConfiguration>? options = null)
            => hostBuilder.ConfigureServices((ctx, services) => services.UseModules(moduleCatalog, options, ctx));
    }
}
