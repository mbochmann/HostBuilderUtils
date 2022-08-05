using HostBuilderUtils.Ioc.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostBuilderUtils.Ioc.DummyModule
{
    public class TestClass
    {
        public TestClass(IModuleManager moduleManager)
        {
            ModuleManager = moduleManager;
        }

        public IModuleManager ModuleManager { get; }
    }
}
