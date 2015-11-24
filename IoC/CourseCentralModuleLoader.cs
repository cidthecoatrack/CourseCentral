using CourseCentral.IoC.Modules;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCentral.IoC
{
    public class CourseCentralModuleLoader
    {
        public void LoadModules(IKernel kernel)
        {
            kernel.Load<DomainModule>();
        }
    }
}
