using CourseCentral.Domain.Repositories;
using Ninject;
using NUnit.Framework;
using System;
using System.Data.SqlClient;

namespace CourseCentral.Tests.Domain.Repositories
{
    [TestFixture]
    public class SqlServerStudentRepositoryTests : DatabaseTests
    {
        [Inject]
        public StudentRepository StudentRepository { get; set; }

        [Test]
        public void FindStudent()
        {
            var sql = @"
                INSERT INTO Students (Id, Name)
                VALUES (@Id, @Name)";

            var newId = Guid.NewGuid();
            var newName = String.Format("Test Student {0}", newId);

            var parameters = new[]
            {
                new SqlParameter("@Id", newId),
                new SqlParameter("@Name", newName)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();

            var student = StudentRepository.Find(newId);
            Assert.That(student.Id, Is.EqualTo(newId));
            Assert.That(student.Name, Is.EqualTo(newName));
        }
    }
}
