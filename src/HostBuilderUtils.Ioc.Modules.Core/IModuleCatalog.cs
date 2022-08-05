
using Microsoft.Extensions.Hosting;

namespace HostBuilderUtils.Ioc.Modules
{
    public interface IModuleCatalog
    {
        ICollection<IModuleConfig> GetModules();
    }
}