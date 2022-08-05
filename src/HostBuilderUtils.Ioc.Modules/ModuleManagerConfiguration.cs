using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostBuilderUtils.Ioc.Modules
{
    public class ModuleManagerConfiguration : IModuleManagerConfiguration
    {
        public ModuleManagerConfiguration(IModuleCatalog modulePathProvider)
        {
            ModulePathProvider = modulePathProvider;
        }
        public IModuleCatalog ModulePathProvider { get; set; }
        /// <inheritdoc/>
        public bool ThrowOnFailure { get; set; } = true;
    }
}
