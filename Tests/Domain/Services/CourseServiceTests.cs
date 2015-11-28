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
    public class CourseServiceTests
    {
        private CourseService service;
        private Mock<CourseRepository> mockCourseRepository;
        private Mock<CourseTakenRepository> mockCourseTakenRepository;
        private Guid courseId;
        private List<CourseTakenModel> coursesTaken;

        [SetUp]
        public void Setup()
        {
            mockCourseRepository = new Mock<CourseRepository>();
            mockCourseTakenRepository = new Mock<CourseTakenRepository>();
            service = new DomainCourseService(mockCourseRepository.Object, mockCourseTakenRepository.Object);
            coursesTaken = new List<CourseTakenModel>();
            courseId = Guid.NewGuid();

            mockCourseTakenRepository.Setup(r => r.FindCourses(courseId)).Returns(coursesTaken);
        }

        [Test]
        public void RemoveCourse()
        {
            service.Remove(courseId);
            mockCourseRepository.Verify(r => r.Remove(courseId), Times.Once);
        }

        [Test]
        public void RemoveCourseWithAnEnrolledStudent()
        {
            var courseTaken = new CourseTakenModel { Student = Guid.NewGuid(), Course = courseId };
            coursesTaken.Add(courseTaken);

            service.Remove(courseId);
            mockCourseTakenRepository.Verify(r => r.Remove(courseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Remove(It.IsAny<CourseTakenModel>()), Times.Once);
            mockCourseRepository.Verify(r => r.Remove(courseId), Times.Once);
            mockCourseRepository.Verify(r => r.Remove(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void RemoveCourseWithEnrolledStudents()
        {
            var courseTaken = new CourseTakenModel { Student = Guid.NewGuid(), Course = courseId };
            var otherCourseTaken = new CourseTakenModel { Student = Guid.NewGuid(), Course = courseId };
            coursesTaken.Add(courseTaken);
            coursesTaken.Add(otherCourseTaken);

            service.Remove(courseId);
            mockCourseTakenRepository.Verify(r => r.Remove(courseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Remove(otherCourseTaken), Times.Once);
            mockCourseTakenRepository.Verify(r => r.Remove(It.IsAny<CourseTakenModel>()), Times.Exactly(2));
            mockCourseRepository.Verify(r => r.Remove(courseId), Times.Once);
            mockCourseRepository.Verify(r => r.Remove(It.IsAny<Guid>()), Times.Once);
        }
    }
}
