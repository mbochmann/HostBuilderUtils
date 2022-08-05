
namespace HostBuilderUtils.Ioc.Modules
{
    public interface IModuleManager
    {
        ICollection<IModuleConfig> GetModules();
        Dictionary<string, IModule> LoadModules();
    }
}