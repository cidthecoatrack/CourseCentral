using CourseCentral.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using Tree;

namespace CourseCentral.Tests.Web.Controllers
{
    [TestFixture]
    public class TreesControllerTests
    {
        private TreesController controller;
        private Mock<BinaryTreeParser> mockBinaryTreeParser;
        private Mock<BinaryTreeSearcher> mockBinaryTreeSearcher;

        [SetUp]
        public void Setup()
        {
            mockBinaryTreeParser = new Mock<BinaryTreeParser>();
            mockBinaryTreeSearcher = new Mock<BinaryTreeSearcher>();
            controller = new TreesController(mockBinaryTreeParser.Object, mockBinaryTreeSearcher.Object);
        }

        [TestCase("Index")]
        [TestCase("Search")]
        public void ActionHandlesGetVerb(String methodName)
        {
            var attributes = AttributeProvider.GetAttributesFor(controller, methodName);
            Assert.That(attributes, Contains.Item(typeof(HttpGetAttribute)));
        }

        [Test]
        public void IndexReturnsView()
        {
            var result = controller.Index();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void SearchReturnsJson()
        {
            var result = controller.Search("search tree", 9266);
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public void SearchJsonAllowsGet()
        {
            var result = controller.Search("search tree", 9266) as JsonResult;
            Assert.That(result.JsonRequestBehavior, Is.EqualTo(JsonRequestBehavior.AllowGet));
        }

        [Test]
        public void SearchJsonContainsLevels()
        {
            var tree = new Node();
            var levels = new[] { 90210, 42 };

            mockBinaryTreeParser.Setup(p => p.Parse("search tree")).Returns(tree);
            mockBinaryTreeSearcher.Setup(s => s.FindLevelsOf(9266, tree)).Returns(levels);

            var result = controller.Search("search tree", 9266) as JsonResult;
            dynamic data = result.Data;
            Assert.That(data.levels, Is.EqualTo(levels));
        }
    }
}
