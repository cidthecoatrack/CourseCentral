using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using Ninject;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace CourseCentral.Tests.Domain.Repositories
{
    [TestFixture]
    public class SqlServerCourseTakenRepositoryTests : DatabaseTests
    {
        [Inject]
        public CourseTakenRepository CourseTakenRepository { get; set; }
        [Inject]
        public StudentRepository StudentRepository { get; set; }
        [Inject]
        public CourseRepository CourseRepository { get; set; }
        [Inject]
        public Random Random { get; set; }

        [Test]
        public void FindCourseTaken()
        {
            var studentId = StudentRepository.FindAll().Last().Id;
            var courseId = CourseRepository.FindAll().Last().Id;
            var newCourseTaken = CreateCourseTaken(studentId, courseId);

            var courseTaken = CourseTakenRepository.Find(newCourseTaken.Student, newCourseTaken.Course);
            AssertCoursesTakenAreEqual(courseTaken, newCourseTaken);
        }

        [Test]
        public void FindCoursesTakenByStudent()
        {
            var studentId = StudentRepository.FindAll().Last().Id;
            var firstCourseId = CourseRepository.FindAll().First().Id;
            var lastCourseId = CourseRepository.FindAll().Last().Id;
            var firstNewCourseTaken = CreateCourseTaken(studentId, firstCourseId);
            var secondNewCourseTaken = CreateCourseTaken(studentId, lastCourseId);

            var coursesTaken = CourseTakenRepository.FindCourses(studentId);
            Assert.That(coursesTaken, Is.Not.Empty);

            var studentIds = coursesTaken.Select(c => c.Student);
            Assert.That(studentIds, Is.All.EqualTo(studentId));

            var firstCourseTaken = coursesTaken.First(c => c.Course == firstNewCourseTaken.Course);
            AssertCoursesTakenAreEqual(firstCourseTaken, firstNewCourseTaken);

            var secondCourseTaken = coursesTaken.First(c => c.Course == secondNewCourseTaken.Course);
            AssertCoursesTakenAreEqual(secondCourseTaken, secondNewCourseTaken);
        }

        private void AssertCoursesTakenAreEqual(CourseTakenModel actual, CourseTakenModel expected)
        {
            Assert.That(actual.Course, Is.EqualTo(expected.Course));
            Assert.That(actual.Student, Is.EqualTo(expected.Student));
            Assert.That(actual.Grade, Is.EqualTo(expected.Grade));
        }

        private CourseTakenModel CreateCourseTaken(Guid student, Guid course)
        {
            var courseTaken = new CourseTakenModel
            {
                Student = student,
                Course = course,
                Grade = Random.Next(110)
            };

            var sql = @"
                INSERT INTO CoursesTaken (Student, Course, Grade)
                VALUES (@Student, @Course, @Grade)";

            var parameters = new[]
            {
                new SqlParameter("@Student", courseTaken.Student),
                new SqlParameter("@Course", courseTaken.Course),
                new SqlParameter("@Grade", courseTaken.Grade)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();

            return courseTaken;
        }
    }
}
