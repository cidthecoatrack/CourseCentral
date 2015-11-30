using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Web.Controllers;
using CourseCentral.Web.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CourseCentral.Tests.Web.Controllers
{
    [TestFixture]
    public class CoursesTakenControllerTests
    {
        private CoursesTakenController controller;
        private Mock<CourseTakenRepository> mockCourseTakenRepository;
        private Mock<CourseRepository> mockCourseRepository;
        private Mock<StudentRepository> mockStudentRepository;

        [SetUp]
        public void Setup()
        {
            mockCourseTakenRepository = new Mock<CourseTakenRepository>();
            mockCourseRepository = new Mock<CourseRepository>();
            mockStudentRepository = new Mock<StudentRepository>();
            controller = new CoursesTakenController(mockCourseTakenRepository.Object, mockCourseRepository.Object, mockStudentRepository.Object);
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

            var students = new[]
            {
                new StudentModel { Id = Guid.NewGuid(), FirstName = "first first name" },
                new StudentModel { Id = studentId, FirstName = "second first name" }
            };

            mockStudentRepository.Setup(r => r.FindAll()).Returns(students);

            var result = controller.Student(studentId);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void StudentViewHasModel()
        {
            var studentId = Guid.NewGuid();

            var students = new[]
            {
                new StudentModel { Id = Guid.NewGuid(), FirstName = "first first name" },
                new StudentModel { Id = studentId, FirstName = "second first name" }
            };

            mockStudentRepository.Setup(r => r.FindAll()).Returns(students);

            var result = controller.Student(studentId) as ViewResult;
            Assert.That(result.Model, Is.InstanceOf<CoursesTakenModel>());
        }

        [Test]
        public void StudentViewHasCoursesTakenByStudent()
        {
            var studentId = Guid.NewGuid();

            var students = new[]
            {
                new StudentModel { Id = Guid.NewGuid(), FirstName = "first first name" },
                new StudentModel { Id = studentId, FirstName = "second first name" }
            };

            mockStudentRepository.Setup(r => r.FindAll()).Returns(students);

            var coursesTaken = new[]
            {
                new CourseTakenModel(),
                new CourseTakenModel()
            };

            mockCourseTakenRepository.Setup(r => r.FindCourses(studentId)).Returns(coursesTaken);

            var result = controller.Student(studentId) as ViewResult;
            var model = result.Model as CoursesTakenModel;

            Assert.That(model.CoursesTaken, Is.EqualTo(coursesTaken));
        }

        [Test]
        public void StudentViewHasAllCourses()
        {
            var studentId = Guid.NewGuid();

            var students = new[]
            {
                new StudentModel { Id = Guid.NewGuid(), FirstName = "first first name" },
                new StudentModel { Id = studentId, FirstName = "second first name" }
            };

            mockStudentRepository.Setup(r => r.FindAll()).Returns(students);

            var courses = new[]
            {
                new CourseModel { Id = Guid.NewGuid(), Name = "first name" },
                new CourseModel { Id = Guid.NewGuid(), Name = "second name" }
            };

            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var result = controller.Student(studentId) as ViewResult;
            var model = result.Model as CoursesTakenModel;
            Assert.That(model.ToAssign.Count(), Is.EqualTo(2));

            var names = model.ToAssign.Select(a => a.Name);
            Assert.That(names, Contains.Item("first name"));
            Assert.That(names, Contains.Item("second name"));

            var ids = model.ToAssign.Select(a => a.Id);
            Assert.That(ids, Contains.Item(courses.First().Id));
            Assert.That(ids, Contains.Item(courses.Last().Id));
        }

        [Test]
        public void StudentViewHasStudent()
        {
            var studentId = Guid.NewGuid();

            var students = new[]
            {
                new StudentModel { Id = Guid.NewGuid(), FirstName = "first first name" },
                new StudentModel { Id = studentId, FirstName = "second first name" }
            };

            mockStudentRepository.Setup(r => r.FindAll()).Returns(students);

            var result = controller.Student(studentId) as ViewResult;
            var model = result.Model as CoursesTakenModel;
            Assert.That(model.Student.Id, Is.EqualTo(studentId));
            Assert.That(model.Student.Name, Is.EqualTo("second first name"));
            Assert.That(model.Course, Is.Null);
        }

        [Test]
        public void CourseReturnsView()
        {
            var courseId = Guid.NewGuid();

            var courses = new[]
            {
                new CourseModel { Id = Guid.NewGuid(), Name = "first name" },
                new CourseModel { Id = courseId, Name = "second name" }
            };

            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var result = controller.Course(courseId);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void CourseViewHasModel()
        {
            var courseId = Guid.NewGuid();

            var courses = new[]
            {
                new CourseModel { Id = Guid.NewGuid(), Name = "first name" },
                new CourseModel { Id = courseId, Name = "second name" }
            };

            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var result = controller.Course(courseId) as ViewResult;
            Assert.That(result.Model, Is.InstanceOf<CoursesTakenModel>());
        }

        [Test]
        public void CourseViewHasStudentsEnrolledInTheCourse()
        {
            var courseId = Guid.NewGuid();

            var courses = new[]
            {
                new CourseModel { Id = Guid.NewGuid(), Name = "first name" },
                new CourseModel { Id = courseId, Name = "second name" }
            };

            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var coursesTaken = new[]
            {
                new CourseTakenModel(),
                new CourseTakenModel()
            };

            mockCourseTakenRepository.Setup(r => r.FindStudents(courseId)).Returns(coursesTaken);

            var result = controller.Course(courseId) as ViewResult;
            var model = result.Model as CoursesTakenModel;

            Assert.That(model.CoursesTaken, Is.EqualTo(coursesTaken));
        }

        [Test]
        public void CourseViewHasAllStudents()
        {
            var courseId = Guid.NewGuid();

            var courses = new[]
            {
                new CourseModel { Id = Guid.NewGuid(), Name = "first name" },
                new CourseModel { Id = courseId, Name = "second name" }
            };

            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var students = new[]
            {
                new StudentModel { Id = Guid.NewGuid(), FirstName = "first first name" },
                new StudentModel { Id = Guid.NewGuid(), FirstName = "second first name" }
            };

            mockStudentRepository.Setup(r => r.FindAll()).Returns(students);

            var result = controller.Course(courseId) as ViewResult;
            var model = result.Model as CoursesTakenModel;
            Assert.That(model.ToAssign.Count(), Is.EqualTo(2));

            var names = model.ToAssign.Select(a => a.Name);
            Assert.That(names, Contains.Item("first first name"));
            Assert.That(names, Contains.Item("second first name"));

            var ids = model.ToAssign.Select(a => a.Id);
            Assert.That(ids, Contains.Item(students.First().Id));
            Assert.That(ids, Contains.Item(students.Last().Id));
        }

        [Test]
        public void CourseViewHasCourse()
        {
            var courseId = Guid.NewGuid();

            var courses = new[]
            {
                new CourseModel { Id = Guid.NewGuid(), Name = "first name" },
                new CourseModel { Id = courseId, Name = "second name" }
            };

            mockCourseRepository.Setup(r => r.FindAll()).Returns(courses);

            var result = controller.Course(courseId) as ViewResult;
            var model = result.Model as CoursesTakenModel;
            Assert.That(model.Course.Id, Is.EqualTo(courseId));
            Assert.That(model.Course.Name, Is.EqualTo("second name"));
            Assert.That(model.Student, Is.Null);
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
