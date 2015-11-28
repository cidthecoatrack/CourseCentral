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
    public class SqlServerCourseRepositoryTests : DatabaseTests
    {
        [Inject]
        public StudentRepository StudentRepository { get; set; }
        [Inject]
        public CourseRepository CourseRepository { get; set; }
        [Inject]
        public CourseTakenRepository CourseTakenRepository { get; set; }
        [Inject]
        public Random Random { get; set; }

        private void AssertCoursesAreEqual(CourseModel actual, CourseModel expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Department, Is.EqualTo(expected.Department));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Number, Is.EqualTo(expected.Number));
            Assert.That(actual.Professor, Is.EqualTo(expected.Professor));
            Assert.That(actual.Section, Is.EqualTo(expected.Section));
            Assert.That(actual.Semester, Is.EqualTo(expected.Semester));
            Assert.That(actual.Year, Is.EqualTo(expected.Year));
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

            var sql = @"
                INSERT INTO Courses (Id, Name, Department, Number, Section, Professor, Year, Semester)
                VALUES (@Id, @Name, @Department, @Number, @Section, @Professor, @Year, @Semester)";

            var parameters = new[]
            {
                new SqlParameter("@Id", course.Id),
                new SqlParameter("@Name", course.Name),
                new SqlParameter("@Department", course.Department),
                new SqlParameter("@Number", course.Number),
                new SqlParameter("@Section", course.Section),
                new SqlParameter("@Professor", course.Professor),
                new SqlParameter("@Year", course.Year),
                new SqlParameter("@Semester", course.Semester)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();

            return course;
        }

        [Test]
        public void FindAllCourses()
        {
            var firstNewCourse = CreateCourse();
            var secondNewCourse = CreateCourse();

            var courses = CourseRepository.FindAll();
            Assert.That(courses, Is.Not.Empty);

            var courseIds = courses.Select(s => s.Id);
            Assert.That(courseIds, Contains.Item(firstNewCourse.Id));
            Assert.That(courseIds, Contains.Item(secondNewCourse.Id));

            var firstCourse = courses.First(s => s.Id == firstNewCourse.Id);
            AssertCoursesAreEqual(firstCourse, firstNewCourse);

            var secondCourse = courses.First(s => s.Id == secondNewCourse.Id);
            AssertCoursesAreEqual(secondCourse, secondNewCourse);
        }

        [Test]
        public void FindNoCourses()
        {
            var courses = CourseRepository.FindAll();

            foreach (var course in courses)
            {
                var coursesTaken = CourseTakenRepository.FindStudents(course.Id);

                foreach (var courseTaken in coursesTaken)
                    CourseTakenRepository.Remove(courseTaken);

                CourseRepository.Remove(course.Id);
            }

            courses = CourseRepository.FindAll();
            Assert.That(courses, Is.Empty);
        }

        [Test]
        public void AddCourse()
        {
            var newCourse = new CourseModel();
            newCourse.Id = Guid.NewGuid();
            newCourse.Name = "Add Course";
            newCourse.Department = "DEPT";
            newCourse.Number = Random.Next();
            newCourse.Professor = String.Format("Professor {0}", newCourse.Id);
            newCourse.Year = DateTime.Now.Year;
            newCourse.Semester = "SPRING";
            newCourse.Section = 'K';

            CourseRepository.Add(newCourse);

            var courses = CourseRepository.FindAll();
            var course = courses.First(c => c.Id == newCourse.Id);
            AssertCoursesAreEqual(course, newCourse);
        }

        [Test]
        public void CannotAddDuplicateCourseId()
        {
            var course = CreateCourse();
            course.Name = "Duplicate Course ID";
            course.Department = "MUSC";
            course.Number = Random.Next();
            course.Professor = Guid.NewGuid().ToString();
            course.Year = 1989;
            course.Semester = "SUMMER";
            course.Section = 'Z';

            Assert.That(() => CourseRepository.Add(course), Throws.Exception);
        }

        [Test]
        public void CannotAddDuplicateCourseNumberForYearAndSemester()
        {
            var course = CreateCourse();
            course.Id = Guid.NewGuid();
            course.Name = "Duplicate Course Number For Year And Semester";
            course.Professor = Guid.NewGuid().ToString();

            Assert.That(() => CourseRepository.Add(course), Throws.Exception);
        }

        [Test]
        public void AddNonDuplicateCourseId()
        {
            var newCourse = CreateCourse();
            newCourse.Id = Guid.NewGuid();
            newCourse.Name = "Non-Duplicate Course ID";
            newCourse.Department = "ASDF";

            CourseRepository.Add(newCourse);

            var courses = CourseRepository.FindAll();
            var course = courses.First(c => c.Id == newCourse.Id);
            AssertCoursesAreEqual(course, newCourse);
        }

        [Test]
        public void SemesterMustBeRealSemester()
        {
            var newCourse = CreateCourse();
            newCourse.Id = Guid.NewGuid();
            newCourse.Department = "ASDF";
            newCourse.Number = Random.Next();
            newCourse.Section = 'X';
            newCourse.Semester = "WINTER";
            newCourse.Name = "Winter Semester Course";

            Assert.That(() => CourseRepository.Add(newCourse), Throws.Exception);

            var courses = CourseRepository.FindAll();
            var courseIds = courses.Select(c => c.Id);
            Assert.That(courseIds, Is.All.Not.EqualTo(newCourse.Id));
        }

        [Test]
        public void RemoveACourse()
        {
            var newCourse = CreateCourse();

            CourseRepository.Remove(newCourse.Id);

            var courses = CourseRepository.FindAll();
            var courseIds = courses.Select(c => c.Id);
            Assert.That(courseIds, Is.All.Not.EqualTo(newCourse.Id));
        }

        [Test]
        public void CannotRemoveACourseWithEnrolledStudents()
        {
            var newStudent = CreateStudent();
            var newCourse = CreateCourse();
            var newCourseTaken = new CourseTakenModel { Student = newStudent.Id, Course = newCourse.Id, Grade = 9266 };
            CourseTakenRepository.Add(newCourseTaken);

            Assert.That(() => CourseRepository.Remove(newCourse.Id), Throws.Exception);

            var courses = CourseRepository.FindAll();
            var course = courses.First(c => c.Id == newCourse.Id);
            AssertCoursesAreEqual(course, newCourse);

            var coursesTaken = CourseTakenRepository.FindCourses(newCourseTaken.Student);
            var courseTaken = coursesTaken.First(c => c.Course == newCourseTaken.Course);
            Assert.That(courseTaken.Course, Is.EqualTo(newCourseTaken.Course));
            Assert.That(courseTaken.Student, Is.EqualTo(newCourseTaken.Student));
            Assert.That(courseTaken.Grade, Is.EqualTo(newCourseTaken.Grade));

            coursesTaken = CourseTakenRepository.FindStudents(newCourseTaken.Course);
            courseTaken = coursesTaken.First(c => c.Student == newCourseTaken.Student);
            Assert.That(courseTaken.Course, Is.EqualTo(newCourseTaken.Course));
            Assert.That(courseTaken.Student, Is.EqualTo(newCourseTaken.Student));
            Assert.That(courseTaken.Grade, Is.EqualTo(newCourseTaken.Grade));
        }

        [Test]
        public void RemoveANonexistantCourse()
        {
            var wrongId = Guid.NewGuid();

            CourseRepository.Remove(wrongId);

            var courses = CourseRepository.FindAll();
            var courseIds = courses.Select(c => c.Id);
            Assert.That(courseIds, Is.All.Not.EqualTo(wrongId));
        }

        [Test]
        public void UpdateACourse()
        {
            var newCourse = CreateCourse();
            newCourse.Name = "new name";
            newCourse.Department = "ASDF";
            newCourse.Number = Random.Next();
            newCourse.Professor = "new professor";
            newCourse.Year = 1989;
            newCourse.Semester = "SUMMER";
            newCourse.Section = 'X';

            CourseRepository.Update(newCourse);

            var courses = CourseRepository.FindAll();
            var course = courses.First(c => c.Id == newCourse.Id);
            AssertCoursesAreEqual(course, newCourse);
        }

        [Test]
        public void UpdateACourseWithSameData()
        {
            var newCourse = CreateCourse();

            CourseRepository.Update(newCourse);

            var courses = CourseRepository.FindAll();
            var course = courses.First(c => c.Id == newCourse.Id);
            AssertCoursesAreEqual(course, newCourse);
        }

        [Test]
        public void DoNotAddANewCourseWhenUpdatingACourse()
        {
            var newCourse = CreateCourse();
            newCourse.Id = Guid.NewGuid();
            newCourse.Name = "new name";
            newCourse.Department = "ASDF";
            newCourse.Number = Random.Next();
            newCourse.Professor = "new professor";
            newCourse.Year = 1989;
            newCourse.Semester = "SUMMER";
            newCourse.Section = 'X';

            CourseRepository.Update(newCourse);

            var courses = CourseRepository.FindAll();
            var courseIds = courses.Select(s => s.Id);
            Assert.That(courseIds, Is.All.Not.EqualTo(newCourse.Id));
        }

        [Test]
        public void DoNotUpdateWithDuplicateCourseNumberInYearAndSemester()
        {
            var courses = CourseRepository.FindAll();
            var firstCourse = courses.First();

            var newCourse = CreateCourse();
            newCourse.Name = "Duplicate Course Number in Year and Semester";
            newCourse.Department = firstCourse.Department;
            newCourse.Number = firstCourse.Number;
            newCourse.Professor = "new professor";
            newCourse.Year = firstCourse.Year;
            newCourse.Semester = firstCourse.Semester;
            newCourse.Section = firstCourse.Section;

            Assert.That(() => CourseRepository.Update(newCourse), Throws.Exception);
        }
    }
}
