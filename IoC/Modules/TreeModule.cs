using Ninject.Modules;
using Tree;
using Tree.Domain;

namespace CourseCentral.IoC.Modules
{
    public class TreeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TreeIndexRepository>().To<DomainTreeIndexRepository>();
            Bind<BinaryTreeParser>().To<DomainBinaryTreeParser>();
            Bind<BinaryTreeSearcher>().To<DomainBinaryTreeSearcher>();
        }
    }
}
