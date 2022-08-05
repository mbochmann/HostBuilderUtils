using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace HostBuilderUtils.Ioc.Modules
{
    public class ModuleManager : IModuleManager
    {
        private readonly IServiceCollection _serviceDescriptors;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly IModuleManagerConfiguration _moduleManagerConfiguration;

        public ModuleManager(IServiceCollection serviceDescriptors, HostBuilderContext hostBuilderContext, IModuleManagerConfiguration moduleManagerConfiguration)
        {
            _serviceDescriptors = serviceDescriptors ?? throw new ArgumentNullException(nameof(serviceDescriptors));
            _hostBuilderContext = hostBuilderContext ?? throw new ArgumentNullException(nameof(hostBuilderContext));
            _moduleManagerConfiguration = moduleManagerConfiguration ?? throw new ArgumentNullException(nameof(moduleManagerConfiguration));
        }
        public ICollection<IModuleConfig> GetModules() => _moduleManagerConfiguration.ModulePathProvider.GetModules();
        public virtual Dictionary<string, IModule> LoadModules()
        {
            Dictionary<string, IModule> result = new Dictionary<string, IModule>();
            try
            {
                var modules = GetModules();
                if (modules != null)
                {
                    var modulesWithoutDependencies = modules.Where(x => (x.Dependencies?.Count ?? 0) == 0).ToList();
                    var loadedModuleConfigs = new List<IModuleConfig>();
                    foreach (var module in modulesWithoutDependencies)
                    {
                        foreach (var item in LoadModule(module))
                            result.Add(item.Key, item.Value);
                        loadedModuleConfigs.Add(module);
                    }
                    var modulesWithDependencies = modules.Except(loadedModuleConfigs).ToList();
                    int maxRuns = modulesWithDependencies.Count;
                    if (maxRuns > 0)
                        for (int i = 0; i < maxRuns; i++)
                        {
                            foreach (var module in modulesWithDependencies)
                            {
                                if (module.Dependencies.Any(x => !result.ContainsKey(x)))
                                    continue;
                                foreach (var item in LoadModule(module))
                                    result.Add(item.Key, item.Value);
                                loadedModuleConfigs.Add(module);
                            }
                            modulesWithDependencies = modules.Except(loadedModuleConfigs).ToList();
                        }
                    if (_moduleManagerConfiguration.ThrowOnFailure)
                    {
                        var remainingModules = modules.Except(loadedModuleConfigs).ToList();
                        if (remainingModules.Count > 0)
                            throw new InvalidOperationException("Can't load all modules, because of missing dependencies: " + String.Join("\r\n", remainingModules.Select(x => x.Assembly + "+" + x.Type)));
                    }
                }
            }
            catch
            {
                if (_moduleManagerConfiguration.ThrowOnFailure)
                    throw;
            }
            return result;
        }
        protected virtual Dictionary<string, IModule> LoadModule(IModuleConfig moduleConfig)
        {
            Dictionary<string, IModule> result = new Dictionary<string, IModule>();
            try
            {
                if (LoadAssembly(moduleConfig.Assembly) is Assembly assembly)
                {
                    if (moduleConfig.AutoDiscover == AutoDiscoverMode.All)
                    {
                        if (moduleConfig.Type != null)
                            throw new ArgumentException(nameof(moduleConfig.Type) + " can not set when " + nameof(AutoDiscoverMode) + " is set to " + nameof(AutoDiscoverMode.All));
                        if (moduleConfig.Name != null)
                            throw new ArgumentException(nameof(moduleConfig.Name) + " can not set when " + nameof(AutoDiscoverMode) + " is set to " + nameof(AutoDiscoverMode.All));
                        var types = assembly.GetTypes().Where(x => x.IsAssignableTo(typeof(IModule))).ToList();
                        foreach (var type in types)
                        {
                            string typeName = type.FullName ?? throw new ArgumentException(nameof(type.FullName));
                            if (InitializeModule(type) is IModule iModule)
                            {
                                iModule.ConfigureServices(_hostBuilderContext, _serviceDescriptors);
                                result.Add(typeName, iModule);
                            }
                        }
                    }
                    else if (moduleConfig.Type is string typeName && moduleConfig.Name is string moduleName && assembly.GetType(typeName) is Type type)
                    {
                        if (InitializeModule(type) is IModule iModule)
                        {
                            iModule.ConfigureServices(_hostBuilderContext, _serviceDescriptors);
                            result.Add(moduleName, iModule);
                        }
                    }
                }
            }
            catch
            {
                if (_moduleManagerConfiguration.ThrowOnFailure)
                    throw;
            }
            return result;
        }
        protected virtual IModule? InitializeModule(Type type)
        {
            try
            {
                if (type.IsAssignableTo(typeof(IModule)))
                    if (Activator.CreateInstance(type) is IModule iModule)
                        return iModule;
            }
            catch
            {
                if (_moduleManagerConfiguration.ThrowOnFailure)
                    throw;
            }
            return null;
        }
        protected virtual Assembly? LoadAssembly(string assemblyPath)
        {
            try
            {
                return Assembly.LoadFrom(assemblyPath);

            }
            catch
            {
                if (_moduleManagerConfiguration.ThrowOnFailure)
                    throw;
            }
            return null;
        }
    }
}