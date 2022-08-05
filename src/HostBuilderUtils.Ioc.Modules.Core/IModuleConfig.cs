
namespace HostBuilderUtils.Ioc.Modules
{
    public enum InstanceType
    {
        Scope = 1,
        Singleton = 2,
    }
    public enum AutoDiscoverMode
    {
        One = 1,
        All = 2,
    }
    public interface IModuleConfig
    {
        string Assembly { get; set; }
        AutoDiscoverMode AutoDiscover { get; set; }
        ICollection<string> Dependencies { get; set; }
        InstanceType Mode { get; set; }
        string? Name { get; set; }
        string? Type { get; set; }
    }
}