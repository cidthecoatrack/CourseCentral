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
    public class StudentsControllerTests
    {
        private StudentsController controller;
        private Mock<StudentRepository> mockStudentRepository;
        private Mock<StudentService> mockStudentService;

        [SetUp]
        public void Setup()
        {
            mockStudentRepository = new Mock<StudentRepository>();
            mockStudentService = new Mock<StudentService>();
            controller = new StudentsController(mockStudentRepository.Object, mockStudentService.Object);
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
            var students = new[] { new StudentModel(), new StudentModel() };
            mockStudentRepository.Setup(r => r.FindAll()).Returns(students);

            var result = controller.FindAll() as JsonResult;
            dynamic data = result.Data;
            Assert.That(data.students, Is.EqualTo(students));
        }

        [Test]
        public void UpdateStudent()
        {
            var student = new StudentModel();

            controller.Update(student);
            mockStudentRepository.Verify(r => r.Update(student), Times.Once);
            mockStudentRepository.Verify(r => r.Update(It.IsAny<StudentModel>()), Times.Once);
        }

        [Test]
        public void AddStudent()
        {
            var student = new StudentModel();

            controller.Add(student);
            mockStudentRepository.Verify(r => r.Add(student), Times.Once);
            mockStudentRepository.Verify(r => r.Add(It.IsAny<StudentModel>()), Times.Once);
        }

        [Test]
        public void RemoveStudent()
        {
            var studentId = Guid.NewGuid();

            controller.Remove(studentId);
            mockStudentService.Verify(r => r.Remove(studentId), Times.Once);
            mockStudentService.Verify(r => r.Remove(It.IsAny<Guid>()), Times.Once);
        }
    }
}
