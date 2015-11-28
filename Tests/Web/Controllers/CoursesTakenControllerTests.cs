using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Web.Controllers;
using CourseCentral.Web.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace CourseCentral.Tests.Web.Controllers
{
    [TestFixture]
    public class CoursesTakenControllerTests
    {
        private CoursesTakenController controller;
        private Mock<CourseTakenRepository> mockCourseTakenRepository;

        [SetUp]
        public void Setup()
        {
            mockCourseTakenRepository = new Mock<CourseTakenRepository>();
            controller = new CoursesTakenController(mockCourseTakenRepository.Object);
        }

        [TestCase("Student")]
        [TestCase("Course")]
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
        public void StudentReturnsView()
        {
            var studentId = Guid.NewGuid();
            var result = controller.Student(studentId);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void StudentViewHasModel()
        {
            var studentId = Guid.NewGuid();
            var result = controller.Student(studentId) as ViewResult;
            Assert.That(result.Model, Is.InstanceOf<CoursesTakenModel>());
        }

        [Test]
        public void StudentViewHasCoursesTakenByStudent()
        {
            var studentId = Guid.NewGuid();
            var coursesTaken = new[]
            {
                new CourseTakenModel { Student = studentId, Course = Guid.NewGuid() },
                new CourseTakenModel { Student = studentId, Course = Guid.NewGuid() }
            };

            mockCourseTakenRepository.Setup(r => r.FindCourses(studentId)).Returns(coursesTaken);

            var result = controller.Student(studentId) as ViewResult;
            var model = result.Model as CoursesTakenModel;

            Assert.That(model.CoursesTaken, Is.EqualTo(coursesTaken));
        }

        [Test]
        public void CourseReturnsView()
        {
            var courseId = Guid.NewGuid();
            var result = controller.Course(courseId);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void CourseViewHasModel()
        {
            var courseId = Guid.NewGuid();
            var result = controller.Course(courseId) as ViewResult;
            Assert.That(result.Model, Is.InstanceOf<CoursesTakenModel>());
        }

        [Test]
        public void CourseViewHasStudentsEnrolledInTheCourse()
        {
            var courseId = Guid.NewGuid();
            var coursesTaken = new[]
            {
                new CourseTakenModel { Student = Guid.NewGuid(), Course = courseId },
                new CourseTakenModel { Student = Guid.NewGuid(), Course = courseId }
            };

            mockCourseTakenRepository.Setup(r => r.FindStudents(courseId)).Returns(coursesTaken);

            var result = controller.Course(courseId) as ViewResult;
            var model = result.Model as CoursesTakenModel;

            Assert.That(model.CoursesTaken, Is.EqualTo(coursesTaken));
        }

        [Test]
        public void UpdateCourseTaken()
        {
            var courseTaken = new CourseTakenModel();

            controller.Update(courseTaken);
            mockCourseTakenRepository.Verify(r => r.Update(courseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Update(It.IsAny<CourseTakenModel>()), Times.Once);
        }

        [Test]
        public void AddCourseTaken()
        {
            var courseTaken = new CourseTakenModel();

            controller.Add(courseTaken);
            mockCourseTakenRepository.Verify(r => r.Add(courseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Add(It.IsAny<CourseTakenModel>()), Times.Once);
        }

        [Test]
        public void RemoveCourseTaken()
        {
            var courseTaken = new CourseTakenModel();

            controller.Remove(courseTaken);
            mockCourseTakenRepository.Verify(r => r.Remove(courseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Remove(It.IsAny<CourseTakenModel>()), Times.Once);
        }
    }
}
