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
            var student = CreateStudent();
            var course = CreateCourse();
            var newCourseTaken = CreateCourseTaken(student.Id, course.Id);

            var courseTaken = CourseTakenRepository.Find(newCourseTaken.Student, newCourseTaken.Course);
            AssertCoursesTakenAreEqual(courseTaken, newCourseTaken);
        }

        private StudentModel CreateStudent()
        {
            var student = new StudentModel();
            student.Id = Guid.NewGuid();
            student.FirstName = String.Format("First Name {0}", student.Id);
            student.MiddleName = String.Format("Middle Name {0}", student.Id);
            student.LastName = String.Format("Last Name {0}", student.Id);
            student.Suffix = String.Format("Suffix {0}", student.Id);
            student.DateOfBirth = DateTime.Now.Date;

            StudentRepository.Add(student);

            return student;
        }

        private CourseModel CreateCourse()
        {
            var course = new CourseModel();
            course.Id = Guid.NewGuid();
            course.Name = String.Format("Name {0}", course.Id);
            course.Department = "DEPT";
            course.Number = Random.Next(1000, 10000);
            course.Professor = String.Format("Professor {0}", course.Id);
            course.Year = DateTime.Now.Year;
            course.Semester = "FALL";
            course.Section = 'A';

            CourseRepository.Add(course);

            return course;
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

        [Test]
        public void DoNotFindCourseTakenByStudent()
        {
            var student = CreateStudent();
            var wrongId = Guid.NewGuid();
            var message = String.Format("Student {0} is not enrolled in course {1}", wrongId);

            Assert.That(() => CourseTakenRepository.Find(student.Id, wrongId), Throws.InstanceOf<InvalidOperationException>().With.Message.EqualTo(message));
        }

        [Test]
        public void FindCoursesTakenByStudent()
        {
            var student = CreateStudent();
            var firstCourse = CreateCourse();
            var lastCourse = CreateCourse();
            var firstNewCourseTaken = CreateCourseTaken(student.Id, firstCourse.Id);
            var secondNewCourseTaken = CreateCourseTaken(student.Id, lastCourse.Id);

            var coursesTaken = CourseTakenRepository.FindCourses(student.Id);
            Assert.That(coursesTaken, Is.Not.Empty);

            var studentIds = coursesTaken.Select(c => c.Student);
            Assert.That(studentIds, Is.All.EqualTo(student.Id));

            var firstCourseTaken = coursesTaken.First(c => c.Course == firstNewCourseTaken.Course);
            AssertCoursesTakenAreEqual(firstCourseTaken, firstNewCourseTaken);

            var secondCourseTaken = coursesTaken.First(c => c.Course == secondNewCourseTaken.Course);
            AssertCoursesTakenAreEqual(secondCourseTaken, secondNewCourseTaken);
        }

        [Test]
        public void FindNoCoursesTakenByStudent()
        {
            throw new NotImplementedException();
        }

        private void AssertCoursesTakenAreEqual(CourseTakenModel actual, CourseTakenModel expected)
        {
            Assert.That(actual.Course, Is.EqualTo(expected.Course));
            Assert.That(actual.Student, Is.EqualTo(expected.Student));
            Assert.That(actual.Grade, Is.EqualTo(expected.Grade));
        }

        [Test]
        public void FindStudentsTakingCourse()
        {
            var course = CreateCourse();
            var firstStudent = CreateStudent();
            var lastStudent = CreateStudent();
            var firstNewCourseTaken = CreateCourseTaken(firstStudent.Id, course.Id);
            var secondNewCourseTaken = CreateCourseTaken(lastStudent.Id, course.Id);

            var coursesTaken = CourseTakenRepository.FindStudents(course.Id);
            Assert.That(coursesTaken, Is.Not.Empty);

            var courseIds = coursesTaken.Select(c => c.Course);
            Assert.That(courseIds, Is.All.EqualTo(course.Id));

            var firstCourseTaken = coursesTaken.First(c => c.Student == firstNewCourseTaken.Student);
            AssertCoursesTakenAreEqual(firstCourseTaken, firstNewCourseTaken);

            var secondCourseTaken = coursesTaken.First(c => c.Student == secondNewCourseTaken.Student);
            AssertCoursesTakenAreEqual(secondCourseTaken, secondNewCourseTaken);
        }

        [Test]
        public void FindNoStudentsTakingCourse()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void AddCourseTaken()
        {
            var student = CreateStudent();
            var course = CreateCourse();

            var newCourseTaken = new CourseTakenModel
            {
                Student = student.Id,
                Course = course.Id,
                Grade = Random.Next()
            };

            CourseTakenRepository.Add(newCourseTaken);

            var courseTaken = CourseTakenRepository.Find(student.Id, course.Id);
            AssertCoursesTakenAreEqual(courseTaken, newCourseTaken);
        }

        [Test]
        public void CannotTakeSameCourseTwice()
        {
            var student = CreateStudent();
            var course = CreateCourse();
            var newCourseTaken = CreateCourseTaken(student.Id, course.Id);

            newCourseTaken.Grade = Random.Next();

            Assert.That(() => CourseTakenRepository.Add(newCourseTaken), Throws.Exception);
        }

        [Test]
        public void RemoveCourseTaken()
        {
            var student = CreateStudent();
            var course = CreateCourse();
            var newCourseTaken = CreateCourseTaken(student.Id, course.Id);
            newCourseTaken.Grade = Random.Next();

            CourseTakenRepository.Remove(newCourseTaken);

            Assert.That(() => CourseTakenRepository.Find(student.Id, course.Id), Throws.Exception);
        }
    }
}
