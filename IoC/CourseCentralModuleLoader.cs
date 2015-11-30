using CourseCentral.IoC.Modules;
using Ninject;

namespace CourseCentral.IoC
{
    public class CourseCentralModuleLoader
    {
        public void LoadModules(IKernel kernel)
        {
            kernel.Load<DomainModule>();
            kernel.Load<TreeModule>();
        }
    }
}
