using CourseCentral.IoC;
using Ninject;
using NUnit.Framework;

namespace CourseCentral.Tests.Tree
{
    [TestFixture]
    public abstract class IntegrationTests
    {
        private IKernel kernel;

        [TestFixtureSetUp]
        public void IntegrationTestsFixtureSetup()
        {
            kernel = new StandardKernel();

            var domainLoader = new CourseCentralModuleLoader();
            domainLoader.LoadModules(kernel);
        }

        [SetUp]
        public void IntegrationTestsSetup()
        {
            kernel.Inject(this);
        }
    }
}
