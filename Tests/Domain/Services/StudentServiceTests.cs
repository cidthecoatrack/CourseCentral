using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Services;
using CourseCentral.Domain.Services.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CourseCentral.Tests.Domain.Services
{
    [TestFixture]
    public class StudentServiceTests
    {
        private StudentService service;
        private Mock<StudentRepository> mockStudentRepository;
        private Mock<CourseTakenRepository> mockCourseTakenRepository;
        private Guid studentId;
        private List<CourseTakenModel> coursesTaken;

        [SetUp]
        public void Setup()
        {
            mockStudentRepository = new Mock<StudentRepository>();
            mockCourseTakenRepository = new Mock<CourseTakenRepository>();
            service = new DomainStudentService(mockStudentRepository.Object, mockCourseTakenRepository.Object);
            coursesTaken = new List<CourseTakenModel>();
            studentId = Guid.NewGuid();

            mockCourseTakenRepository.Setup(r => r.FindCourses(studentId)).Returns(coursesTaken);
        }

        [Test]
        public void RemoveStudent()
        {
            service.Remove(studentId);
            mockStudentRepository.Verify(r => r.Remove(studentId), Times.Once);
        }

        [Test]
        public void RemoveStudentEnrolledInAClass()
        {
            var courseTaken = new CourseTakenModel { Student = studentId, Course = Guid.NewGuid() };
            coursesTaken.Add(courseTaken);

            service.Remove(studentId);
            mockCourseTakenRepository.Verify(r => r.Remove(courseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Remove(It.IsAny<CourseTakenModel>()), Times.Once);
            mockStudentRepository.Verify(r => r.Remove(studentId), Times.Once);
            mockStudentRepository.Verify(r => r.Remove(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void RemoveStudentEnrolledInMultipleClasses()
        {
            var courseTaken = new CourseTakenModel { Student = studentId, Course = Guid.NewGuid() };
            var otherCourseTaken = new CourseTakenModel { Student = studentId, Course = Guid.NewGuid() };
            coursesTaken.Add(courseTaken);
            coursesTaken.Add(otherCourseTaken);

            service.Remove(studentId);
            mockCourseTakenRepository.Verify(r => r.Remove(courseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Remove(otherCourseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Remove(It.IsAny<CourseTakenModel>()), Times.Exactly(2));
            mockStudentRepository.Verify(r => r.Remove(studentId), Times.Once);
            mockStudentRepository.Verify(r => r.Remove(It.IsAny<Guid>()), Times.Once);
        }
    }
}
