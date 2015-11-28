using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Services;
using CourseCentral.Web.Controllers;
using CourseCentral.Web.Models;
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
        public void IndexViewHasModel()
        {
            var result = controller.Index() as ViewResult;
            Assert.That(result.Model, Is.InstanceOf<CoursesModel>());
        }

        [Test]
        public void IndexViewHasCourses()
        {
            var courses = new[]
            {
                new CourseModel { Id = Guid.NewGuid() },
                new CourseModel { Id = Guid.NewGuid() }
            };

            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var result = controller.Index() as ViewResult;
            var model = result.Model as CoursesModel;

            Assert.That(model.Courses, Is.EqualTo(courses));
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
