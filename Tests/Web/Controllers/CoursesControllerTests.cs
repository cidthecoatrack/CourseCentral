using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Services;
using CourseCentral.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace CourseCentral.Tests.Web.Controllers
{
    [TestFixture]
    public class CoursesControllerTests
    {
        private CoursesController controller;
        private Mock<CourseRepository> mockCourseRepository;
        private Mock<CourseService> mockCourseService;

        [SetUp]
        public void Setup()
        {
            mockCourseRepository = new Mock<CourseRepository>();
            mockCourseService = new Mock<CourseService>();
            controller = new CoursesController(mockCourseRepository.Object, mockCourseService.Object);
        }

        [TestCase("Index")]
        [TestCase("FindAll")]
        public void ActionHandlesGetVerb(String methodName)
        {
            var attributes = AttributeProvider.GetAttributesFor(controller, methodName);
            Assert.That(attributes, Contains.Item(typeof(HttpGetAttribute)));
        }

        [TestCase("Update")]
        [TestCase("Add")]
        [TestCase("Remove")]
        public void ActionHandlesPostVerb(String methodName)
        {
            var attributes = AttributeProvider.GetAttributesFor(controller, methodName);
            Assert.That(attributes, Contains.Item(typeof(HttpPostAttribute)));
        }

        [Test]
        public void IndexReturnsView()
        {
            var result = controller.Index();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void FindAllReturnsJsonResult()
        {
            var result = controller.FindAll();
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public void FindAllJsonResultAllowsGet()
        {
            var result = controller.FindAll() as JsonResult;
            Assert.That(result.JsonRequestBehavior, Is.EqualTo(JsonRequestBehavior.AllowGet));
        }

        [Test]
        public void FindAllReturnsStudents()
        {
            var courses = new[] { new CourseModel(), new CourseModel() };
            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var result = controller.FindAll() as JsonResult;
            dynamic data = result.Data;
            Assert.That(data.courses, Is.EqualTo(courses));
        }

        [Test]
        public void UpdateStudent()
        {
            var course = new CourseModel();

            controller.Update(course);
            mockCourseRepository.Verify(r => r.Update(course), Times.Once);
            mockCourseRepository.Verify(r => r.Update(It.IsAny<CourseModel>()), Times.Once);
        }

        [Test]
        public void AddStudent()
        {
            var course = new CourseModel();

            controller.Add(course);
            mockCourseRepository.Verify(r => r.Add(course), Times.Once);
            mockCourseRepository.Verify(r => r.Add(It.IsAny<CourseModel>()), Times.Once);
        }

        [Test]
        public void RemoveStudent()
        {
            var courseId = Guid.NewGuid();

            controller.Remove(courseId);
            mockCourseService.Verify(r => r.Remove(courseId), Times.Once);
            mockCourseService.Verify(r => r.Remove(It.IsAny<Guid>()), Times.Once);
        }
    }
}
