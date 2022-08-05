using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostBuilderUtils.Ioc.Modules
{

    public class ModuleConfig : IModuleConfig
    {
        public string? Name { get; set; }
        public string Assembly { get; set; } = "";
        public AutoDiscoverMode AutoDiscover { get; set; } = AutoDiscoverMode.One;
        public string? Type { get; set; }
        public InstanceType Mode { get; set; } = InstanceType.Singleton;
        public ICollection<string> Dependencies { get; set; } = new List<string>();
    }
}
