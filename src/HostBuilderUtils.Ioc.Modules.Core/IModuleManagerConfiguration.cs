namespace HostBuilderUtils.Ioc.Modules
{
    public interface IModuleManagerConfiguration
    {
        IModuleCatalog ModulePathProvider { get; set; }
        /// <summary>
        /// If true, failures while loading modules will throw an exception. Otherwise failures are silent.
        /// </summary>
        bool ThrowOnFailure { get; set; }
    }
}