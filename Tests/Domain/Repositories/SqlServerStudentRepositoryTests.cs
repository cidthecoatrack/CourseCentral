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
    public class SqlServerStudentRepositoryTests : DatabaseTests
    {
        [Inject]
        public StudentRepository StudentRepository { get; set; }
        [Inject]
        public CourseRepository CourseRepository { get; set; }
        [Inject]
        public CourseTakenRepository CourseTakenRepository { get; set; }
        [Inject]
        public Random Random { get; set; }

        private void AssertStudentsAreEqual(StudentModel actual, StudentModel expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.FirstName, Is.EqualTo(expected.FirstName));
            Assert.That(actual.MiddleName, Is.EqualTo(expected.MiddleName));
            Assert.That(actual.LastName, Is.EqualTo(expected.LastName));
            Assert.That(actual.Suffix, Is.EqualTo(expected.Suffix));
            Assert.That(actual.DateOfBirth, Is.EqualTo(expected.DateOfBirth));
        }

        private StudentModel CreateStudent()
        {
            var sql = @"
                INSERT INTO Students (Id, FirstName, MiddleName, LastName, Suffix, DateOfBirth)
                VALUES (@Id, @FirstName, @MiddleName, @LastName, @Suffix, @DateOfBirth)";

            var student = new StudentModel();
            student.Id = Guid.NewGuid();
            student.FirstName = String.Format("First Name {0}", student.Id);
            student.MiddleName = String.Format("Middle Name {0}", student.Id);
            student.LastName = String.Format("Last Name {0}", student.Id);
            student.Suffix = String.Format("Suffix {0}", student.Id);
            student.DateOfBirth = DateTime.Now.Date;

            var parameters = new[]
            {
                new SqlParameter("@Id", student.Id),
                new SqlParameter("@FirstName", student.FirstName),
                new SqlParameter("@MiddleName", student.MiddleName),
                new SqlParameter("@LastName", student.LastName),
                new SqlParameter("@Suffix", student.Suffix),
                new SqlParameter("@DateOfBirth", student.DateOfBirth)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();

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

        [Test]
        public void FindAllStudents()
        {
            var firstNewStudent = CreateStudent();
            var secondNewStudent = CreateStudent();

            var students = StudentRepository.FindAll();
            Assert.That(students, Is.Not.Empty);

            var studentIds = students.Select(s => s.Id);
            Assert.That(studentIds, Contains.Item(firstNewStudent.Id));
            Assert.That(studentIds, Contains.Item(secondNewStudent.Id));

            var firstStudent = students.First(s => s.Id == firstNewStudent.Id);
            AssertStudentsAreEqual(firstStudent, firstNewStudent);

            var secondStudent = students.First(s => s.Id == secondNewStudent.Id);
            AssertStudentsAreEqual(secondStudent, secondNewStudent);
        }

        [Test]
        public void FindNoStudents()
        {
            var students = StudentRepository.FindAll();

            foreach (var student in students)
            {
                var coursesTaken = CourseTakenRepository.FindCourses(student.Id);

                foreach (var courseTaken in coursesTaken)
                    CourseTakenRepository.Remove(courseTaken);

                StudentRepository.Remove(student.Id);
            }

            students = StudentRepository.FindAll();
            Assert.That(students, Is.Empty);
        }

        [Test]
        public void AddStudent()
        {
            var newStudent = new StudentModel();
            newStudent.Id = Guid.NewGuid();
            newStudent.FirstName = String.Format("First Name {0}", newStudent.Id);
            newStudent.MiddleName = String.Format("Middle Name {0}", newStudent.Id);
            newStudent.LastName = String.Format("Last Name {0}", newStudent.Id);
            newStudent.Suffix = String.Format("Suffix {0}", newStudent.Id);
            newStudent.DateOfBirth = DateTime.Now.Date;

            StudentRepository.Add(newStudent);

            var students = StudentRepository.FindAll();
            var student = students.First(s => s.Id == newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);
        }

        [Test]
        public void CannotAddDuplicateStudentId()
        {
            var student = CreateStudent();
            student.FirstName = Guid.NewGuid().ToString();
            student.LastName = Guid.NewGuid().ToString();
            student.MiddleName = Guid.NewGuid().ToString();
            student.Suffix = Guid.NewGuid().ToString();
            student.DateOfBirth = DateTime.Now.Date;

            Assert.That(() => StudentRepository.Add(student), Throws.Exception);
        }

        [Test]
        public void AddNonDuplicateStudentId()
        {
            var newStudent = CreateStudent();
            newStudent.Id = Guid.NewGuid();

            StudentRepository.Add(newStudent);

            var students = StudentRepository.FindAll();
            var student = students.First(s => s.Id == newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);
        }

        [Test]
        public void RemoveAStudent()
        {
            var newStudent = CreateStudent();
            StudentRepository.Remove(newStudent.Id);

            var students = StudentRepository.FindAll();
            var studentIds = students.Select(s => s.Id);
            Assert.That(studentIds, Is.All.Not.EqualTo(newStudent.Id));
        }

        [Test]
        public void CannotRemoveAStudentEnrolledInClasses()
        {
            var newStudent = CreateStudent();
            var newCourse = CreateCourse();
            var newCourseTaken = new CourseTakenModel { Student = newStudent.Id, Course = newCourse.Id, Grade = Random.Next() };
            CourseTakenRepository.Add(newCourseTaken);

            Assert.That(() => StudentRepository.Remove(newStudent.Id), Throws.Exception);

            var students = StudentRepository.FindAll();
            var student = students.First(s => s.Id == newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);

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
        public void RemoveANonexistantStudent()
        {
            var wrongId = Guid.NewGuid();
            StudentRepository.Remove(wrongId);

            var students = StudentRepository.FindAll();
            var studentIds = students.Select(s => s.Id);
            Assert.That(studentIds, Is.All.Not.EqualTo(wrongId));
        }

        [Test]
        public void UpdateAStudent()
        {
            var newStudent = CreateStudent();
            newStudent.FirstName = "New first name";
            newStudent.MiddleName = "New middle name";
            newStudent.LastName = "New last name";
            newStudent.Suffix = "New suffix";
            newStudent.DateOfBirth = new DateTime(1989, 10, 29);

            StudentRepository.Update(newStudent);

            var students = StudentRepository.FindAll();
            var student = students.First(s => s.Id == newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);
        }

        [Test]
        public void UpdateAStudentWithSameData()
        {
            var newStudent = CreateStudent();

            StudentRepository.Update(newStudent);

            var students = StudentRepository.FindAll();
            var student = students.First(s => s.Id == newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);
        }

        [Test]
        public void DoNotAddANewStudentWhenUpdatingAStudent()
        {
            var newStudent = CreateStudent();
            newStudent.Id = Guid.NewGuid();
            newStudent.FirstName = "New first name";
            newStudent.MiddleName = "New middle name";
            newStudent.LastName = "New last name";
            newStudent.Suffix = "New suffix";
            newStudent.DateOfBirth = new DateTime(1989, 10, 29);

            var students = StudentRepository.FindAll();
            var studentIds = students.Select(s => s.Id);
            Assert.That(studentIds, Is.All.Not.EqualTo(newStudent.Id));
        }
    }
}
