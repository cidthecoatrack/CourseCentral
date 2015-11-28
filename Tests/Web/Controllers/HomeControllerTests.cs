using CourseCentral.Web.Controllers;
using NUnit.Framework;
using System.Web.Mvc;

namespace CourseCentral.Tests.Web.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController controller;

        [SetUp]
        public void Setup()
        {
            controller = new HomeController();
        }

        [Test]
        public void IndexHandlesGetVerb()
        {
            var attributes = AttributeProvider.GetAttributesFor(controller, "Index");
            Assert.That(attributes, Contains.Item(typeof(HttpGetAttribute)));
        }

        [Test]
        public void HomeReturnsView()
        {
            var result = controller.Index();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }
    }
}
