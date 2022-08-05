using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostBuilderUtils.Ioc.Modules
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseModules(this IServiceCollection services, Action<IModuleManagerConfiguration>? options = null, HostBuilderContext? hostBuilderContext = null)
        {
            hostBuilderContext = GetHostBuilderContextSafe(services, hostBuilderContext);
            return UseModules(services, InitializeDefaultModuleCatalog(services, hostBuilderContext), options, hostBuilderContext);
        }
        private static IModuleCatalog InitializeDefaultModuleCatalog(IServiceCollection services, HostBuilderContext? hostBuilderContext = null)
        {
            return new AppSettingsModuleCatalog(GetHostBuilderContextSafe(services, hostBuilderContext));
        }
        private static HostBuilderContext GetHostBuilderContextSafe(IServiceCollection services, HostBuilderContext? hostBuilderContext)
        {
            if (hostBuilderContext is null)
            {
                hostBuilderContext = (services.FirstOrDefault(x => x.ServiceType == typeof(HostBuilderContext))?.ImplementationInstance as HostBuilderContext)!;
                if (hostBuilderContext is null)
                    throw new ArgumentNullException(nameof(hostBuilderContext), "Can not resolve " + nameof(HostBuilderContext) + " automatically. Add it explicitly.");
            }
            return hostBuilderContext;
        }
        public static IServiceCollection UseModules(this IServiceCollection services, IModuleCatalog moduleCatalog, Action<IModuleManagerConfiguration>? options = null, HostBuilderContext? hostBuilderContext = null)
        {
            if (moduleCatalog is null)
                throw new ArgumentNullException(nameof(moduleCatalog));
            hostBuilderContext = GetHostBuilderContextSafe(services, hostBuilderContext);
            var managerConfig = new ModuleManagerConfiguration(moduleCatalog);
            if (options != null)
                options(managerConfig);
            var moduleManager = new ModuleManager(services, hostBuilderContext, managerConfig);
            services
                 .AddSingleton(moduleCatalog)
                 .AddSingleton<IModuleManagerConfiguration>(managerConfig)
                 .AddSingleton<IModuleManager>(moduleManager);
            moduleManager.LoadModules();
            return services;
        }
    }
}
