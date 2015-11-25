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
        public CourseTakenRepository CourseTakenRepository { get; set; }
        [Inject]
        public Random Random { get; set; }

        [Test]
        public void FindStudent()
        {
            var newStudent = CreateStudent();

            var student = StudentRepository.Find(newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);
        }

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

            var newId = Guid.NewGuid();
            var newFirstName = String.Format("First Name {0}", newId);
            var newMiddleName = String.Format("Middle Name {0}", newId);
            var newLastName = String.Format("Last Name {0}", newId);
            var newSuffix = String.Format("Suffix {0}", newId);
            var newDateOfBirth = DateTime.Now.Date;

            var parameters = new[]
            {
                new SqlParameter("@Id", newId),
                new SqlParameter("@FirstName", newFirstName),
                new SqlParameter("@MiddleName", newMiddleName),
                new SqlParameter("@LastName", newLastName),
                new SqlParameter("@Suffix", newSuffix),
                new SqlParameter("@DateOfBirth", newDateOfBirth)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();

            return new StudentModel
            {
                Id = newId,
                FirstName = newFirstName,
                MiddleName = newMiddleName,
                LastName = newLastName,
                Suffix = newSuffix,
                DateOfBirth = newDateOfBirth
            };
        }

        private CourseModel CreateCourse()
        {
            var sql = @"
                INSERT INTO Courses (Id, Name, Department, Number, Professor, Year, Semester)
                VALUES (@Id, @Name, @Department, @Number, @Professor, @Year, @Semester)";

            var newId = Guid.NewGuid();
            var newName = String.Format("Name {0}", newId);
            var newDepartment = "DEPT";
            var newNumber = Random.Next(1000, 10000);
            var newProfessor = String.Format("Professor {0}", newId);
            var newYear = DateTime.Now.Year;
            var newSemester = "FALL";

            var parameters = new[]
            {
                new SqlParameter("@Id", newId),
                new SqlParameter("@Name", newName),
                new SqlParameter("@Department", newDepartment),
                new SqlParameter("@Number", newNumber),
                new SqlParameter("@Professor", newProfessor),
                new SqlParameter("@Year", newYear),
                new SqlParameter("@Semester", newSemester)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();

            return new CourseModel
            {
                Id = newId,
                Name = newName,
                Department = newDepartment,
                Number = newNumber,
                Professor = newProfessor,
                Year = newYear,
                Section = 'A',
                Semester = newSemester
            };
        }

        [Test]
        public void ThrowExceptionIfStudentCannotBeFound()
        {
            var wrongId = Guid.NewGuid();
            var message = String.Format("No student with ID {0} exists", wrongId);

            Assert.That(() => StudentRepository.Find(wrongId), Throws.InstanceOf<InvalidOperationException>().With.Message.EqualTo(message));
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
                StudentRepository.Remove(student.Id);

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

            var student = StudentRepository.Find(newStudent.Id);
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

            var student = StudentRepository.Find(newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);
        }

        [Test]
        public void RemoveAStudent()
        {
            var newStudent = CreateStudent();
            StudentRepository.Remove(newStudent.Id);
            Assert.That(() => StudentRepository.Find(newStudent.Id), Throws.Exception);
        }

        [Test]
        public void RemoveAStudentEnrolledInClasses()
        {
            var newStudent = CreateStudent();
            var newCourse = CreateCourse();
            var courseTaken = new CourseTakenModel { Student = newStudent.Id, Course = newCourse.Id, Grade = 9266 };
            CourseTakenRepository.Add(courseTaken);

            StudentRepository.Remove(newStudent.Id);

            Assert.That(() => StudentRepository.Find(newStudent.Id), Throws.Exception);

            var coursesTaken = CourseTakenRepository.FindCourses(newStudent.Id);
            Assert.That(coursesTaken, Is.Empty);
        }

        [Test]
        public void RemoveANonexistantStudent()
        {
            var wrongId = Guid.NewGuid();
            StudentRepository.Remove(wrongId);
            Assert.That(() => StudentRepository.Find(wrongId), Throws.Exception);
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

            var student = StudentRepository.Find(newStudent.Id);
            AssertStudentsAreEqual(student, newStudent);
        }

        [Test]
        public void UpdateAStudentWithSameData()
        {
            var newStudent = CreateStudent();

            StudentRepository.Update(newStudent);

            var student = StudentRepository.Find(newStudent.Id);
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
