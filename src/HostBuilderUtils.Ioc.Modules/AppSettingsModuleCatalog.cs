using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HostBuilderUtils.Ioc.Modules
{
    public class AppSettingsModuleCatalog : IModuleCatalog
    {
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly AppSettingsModuleCatalogOptions _providerOptions;

        public AppSettingsModuleCatalog(HostBuilderContext hostBuilderContext, AppSettingsModuleCatalogOptions? providerOptions = null)
        {
            _hostBuilderContext = hostBuilderContext;
            _providerOptions = providerOptions ?? new AppSettingsModuleCatalogOptions();
        }
        public ICollection<IModuleConfig> GetModules()
        {
            if (_hostBuilderContext.Configuration.GetSection(_providerOptions.Section) is IConfigurationSection section)
                if (section.Get<ICollection<ModuleConfig>>() is ICollection<ModuleConfig> modules)
                    return modules.Cast<IModuleConfig>().ToList();
            return new List<IModuleConfig>();
        }
    }
}