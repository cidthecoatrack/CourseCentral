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

        private StudentModel student;
        private CourseModel course;

        [SetUp]
        public void Setup()
        {
            student = CreateStudent();
            course = CreateCourse();
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
            course.Number = Random.Next();
            course.Professor = String.Format("Professor {0}", course.Id);
            course.Year = DateTime.Now.Year;
            course.Semester = "FALL";
            course.Section = 'A';

            CourseRepository.Add(course);

            return course;
        }

        private CourseTakenModel CreateCourseTaken(StudentModel student, CourseModel course)
        {
            var courseTaken = new CourseTakenModel
            {
                Student = new NameModel { Id = student.Id, Name = student.FirstName },
                Course = new NameModel { Id = course.Id, Name = course.Name },
                Grade = Random.Next(110)
            };

            var sql = @"
                INSERT INTO CoursesTaken (Student, Course, Grade)
                VALUES (@Student, @Course, @Grade)";

            var parameters = new[]
            {
                new SqlParameter("@Student", courseTaken.Student.Id),
                new SqlParameter("@Course", courseTaken.Course.Id),
                new SqlParameter("@Grade", courseTaken.Grade)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();

            return courseTaken;
        }

        [Test]
        public void FindCoursesTakenByStudent()
        {
            var otherCourse = CreateCourse();
            var firstNewCourseTaken = CreateCourseTaken(student, course);
            var secondNewCourseTaken = CreateCourseTaken(student, otherCourse);

            var coursesTaken = CourseTakenRepository.FindCourses(student.Id);
            Assert.That(coursesTaken, Is.Not.Empty);

            var studentIds = coursesTaken.Select(c => c.Student.Id);
            Assert.That(studentIds, Is.All.EqualTo(student.Id));

            var firstCourseTaken = coursesTaken.First(c => c.Course.Id == firstNewCourseTaken.Course.Id);
            AssertCoursesTakenAreEqual(firstCourseTaken, firstNewCourseTaken);

            var secondCourseTaken = coursesTaken.First(c => c.Course.Id == secondNewCourseTaken.Course.Id);
            AssertCoursesTakenAreEqual(secondCourseTaken, secondNewCourseTaken);
        }

        [Test]
        public void FindNoCoursesTakenByStudent()
        {
            var otherCourse = CreateCourse();

            var coursesTaken = CourseTakenRepository.FindCourses(student.Id);
            Assert.That(coursesTaken, Is.Empty);
        }

        private void AssertCoursesTakenAreEqual(CourseTakenModel actual, CourseTakenModel expected)
        {
            Assert.That(actual.Course.Id, Is.EqualTo(expected.Course.Id));
            Assert.That(actual.Course.Name, Is.EqualTo(expected.Course.Name));
            Assert.That(actual.Student.Id, Is.EqualTo(expected.Student.Id));
            Assert.That(actual.Student.Name, Is.EqualTo(expected.Student.Name));
            Assert.That(actual.Grade, Is.EqualTo(expected.Grade));
        }

        [Test]
        public void FindStudentsTakingCourse()
        {
            var otherStudent = CreateStudent();
            var firstNewCourseTaken = CreateCourseTaken(student, course);
            var secondNewCourseTaken = CreateCourseTaken(otherStudent, course);

            var coursesTaken = CourseTakenRepository.FindStudents(course.Id);
            Assert.That(coursesTaken, Is.Not.Empty);

            var courseIds = coursesTaken.Select(c => c.Course.Id);
            Assert.That(courseIds, Is.All.EqualTo(course.Id));

            var firstCourseTaken = coursesTaken.First(c => c.Student.Id == firstNewCourseTaken.Student.Id);
            AssertCoursesTakenAreEqual(firstCourseTaken, firstNewCourseTaken);

            var secondCourseTaken = coursesTaken.First(c => c.Student.Id == secondNewCourseTaken.Student.Id);
            AssertCoursesTakenAreEqual(secondCourseTaken, secondNewCourseTaken);
        }

        [Test]
        public void FindNoStudentsTakingCourse()
        {
            var otherStudent = CreateStudent();

            var coursesTaken = CourseTakenRepository.FindStudents(course.Id);
            Assert.That(coursesTaken, Is.Empty);
        }

        [Test]
        public void AddCourseTaken()
        {
            var newCourseTaken = new CourseTakenModel
            {
                Student = new NameModel { Id = student.Id, Name = student.FirstName },
                Course = new NameModel { Id = course.Id, Name = course.Name },
                Grade = Random.Next(110)
            };

            CourseTakenRepository.Add(newCourseTaken);

            var coursesTaken = CourseTakenRepository.FindCourses(newCourseTaken.Student.Id);
            var courseTaken = coursesTaken.First(c => c.Course.Id == newCourseTaken.Course.Id);
            AssertCoursesTakenAreEqual(courseTaken, newCourseTaken);

            coursesTaken = CourseTakenRepository.FindStudents(newCourseTaken.Course.Id);
            courseTaken = coursesTaken.First(c => c.Student.Id == newCourseTaken.Student.Id);
            AssertCoursesTakenAreEqual(courseTaken, newCourseTaken);
        }

        [Test]
        public void CannotTakeSameCourseTwice()
        {
            var newCourseTaken = CreateCourseTaken(student, course);

            newCourseTaken.Grade = Random.Next();

            Assert.That(() => CourseTakenRepository.Add(newCourseTaken), Throws.Exception);
        }

        [Test]
        public void RemoveCourseTaken()
        {
            var newCourseTaken = CreateCourseTaken(student, course);
            newCourseTaken.Grade = Random.Next();

            CourseTakenRepository.Remove(newCourseTaken);

            var coursesTaken = CourseTakenRepository.FindCourses(newCourseTaken.Student.Id);
            Assert.That(coursesTaken, Is.Empty);

            coursesTaken = CourseTakenRepository.FindStudents(newCourseTaken.Course.Id);
            Assert.That(coursesTaken, Is.Empty);
        }

        [Test]
        public void DoNotRemoveInaccurateCourseTaken()
        {
            var otherStudent = CreateStudent();
            var otherCourse = CreateCourse();
            var newFirstCourseTaken = CreateCourseTaken(student, course);
            var newSecondCourseTaken = CreateCourseTaken(otherStudent, otherCourse);

            var firstWrongCourseTaken = new CourseTakenModel { Course = newFirstCourseTaken.Course, Student = newSecondCourseTaken.Student };
            var secondWrongCourseTaken = new CourseTakenModel { Course = newSecondCourseTaken.Course, Student = newFirstCourseTaken.Student };

            CourseTakenRepository.Remove(firstWrongCourseTaken);
            CourseTakenRepository.Remove(secondWrongCourseTaken);

            var coursesTaken = CourseTakenRepository.FindCourses(student.Id);
            var courseTaken = coursesTaken.First(c => c.Course.Id == course.Id);
            AssertCoursesTakenAreEqual(courseTaken, newFirstCourseTaken);

            coursesTaken = CourseTakenRepository.FindStudents(course.Id);
            courseTaken = coursesTaken.First(c => c.Student.Id == student.Id);
            AssertCoursesTakenAreEqual(courseTaken, newFirstCourseTaken);

            coursesTaken = CourseTakenRepository.FindCourses(otherStudent.Id);
            courseTaken = coursesTaken.First(c => c.Course.Id == otherCourse.Id);
            AssertCoursesTakenAreEqual(courseTaken, newSecondCourseTaken);

            coursesTaken = CourseTakenRepository.FindStudents(otherCourse.Id);
            courseTaken = coursesTaken.First(c => c.Student.Id == otherStudent.Id);
            AssertCoursesTakenAreEqual(courseTaken, newSecondCourseTaken);
        }

        [Test]
        public void UpdateCourseTaken()
        {
            var newCourseTaken = CreateCourseTaken(student, course);
            newCourseTaken.Grade = Random.Next();

            CourseTakenRepository.Update(newCourseTaken);

            var coursesTaken = CourseTakenRepository.FindCourses(newCourseTaken.Student.Id);
            var courseTaken = coursesTaken.First(c => c.Course.Id == newCourseTaken.Course.Id);
            AssertCoursesTakenAreEqual(courseTaken, newCourseTaken);

            coursesTaken = CourseTakenRepository.FindStudents(newCourseTaken.Course.Id);
            courseTaken = coursesTaken.First(c => c.Student.Id == newCourseTaken.Student.Id);
            AssertCoursesTakenAreEqual(courseTaken, newCourseTaken);
        }

        [Test]
        public void CannotUpdateCourseTakenWithNegativeGrade()
        {
            var newCourseTaken = CreateCourseTaken(student, course);
            newCourseTaken.Grade = -1;

            Assert.That(() => CourseTakenRepository.Update(newCourseTaken), Throws.Exception);
        }

        [Test]
        public void GradeCannotBeNegative()
        {
            var newCourseTaken = new CourseTakenModel
            {
                Student = new NameModel { Id = student.Id, Name = student.FirstName },
                Course = new NameModel { Id = course.Id, Name = course.Name },
                Grade = -1
            };

            Assert.That(() => CourseTakenRepository.Add(newCourseTaken), Throws.Exception);
        }

        [Test]
        public void CannotAddCourseTakenWithWrongStudent()
        {
            var newCourseTaken = new CourseTakenModel
            {
                Student = new NameModel { Id = student.Id, Name = student.FirstName },
                Course = new NameModel { Id = student.Id, Name = student.FirstName },
                Grade = Random.Next()
            };

            Assert.That(() => CourseTakenRepository.Add(newCourseTaken), Throws.Exception);
        }

        [Test]
        public void CannotAddCourseTakenWithWrongCourse()
        {
            var newCourseTaken = new CourseTakenModel
            {
                Student = new NameModel { Id = course.Id, Name = course.Name },
                Course = new NameModel { Id = course.Id, Name = course.Name },
                Grade = Random.Next()
            };

            Assert.That(() => CourseTakenRepository.Add(newCourseTaken), Throws.Exception);
        }
    }
}
