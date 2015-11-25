using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CourseCentral.Domain.Repositories.Domain
{
    public class SqlServerStudentRepository : Repository, StudentRepository
    {
        public SqlServerStudentRepository(String connectionString)
            : base(connectionString)
        {
        }

        public void Add(StudentModel student)
        {
            var sql = @"
                INSERT INTO Students (Id, FirstName, MiddleName, LastName, Suffix, DateOfBirth)
                VALUES (@Id, @FirstName, @MiddleName, @LastName, @Suffix, @DateOfBirth)";

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
        }

        public StudentModel Find(Guid id)
        {
            var sql = @"SELECT *
                        FROM Students
                        WHERE Id = @Id";

            var parameter = new SqlParameter("@Id", id);

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameter))
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    var message = String.Format("No student with ID {0} exists", id);
                    throw new InvalidOperationException(message);
                }

                var student = new StudentModel();
                student.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                student.FirstName = Convert.ToString(reader["FirstName"]);
                student.Id = Guid.Parse(Convert.ToString(reader["Id"]));
                student.LastName = Convert.ToString(reader["LastName"]);
                student.MiddleName = Convert.ToString(reader["MiddleName"]);
                student.Suffix = Convert.ToString(reader["Suffix"]);

                return student;
            }
        }

        public IEnumerable<StudentModel> FindAll()
        {
            var sql = @"SELECT * FROM Students";
            var students = new List<StudentModel>();

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var student = new StudentModel();
                    student.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    student.FirstName = Convert.ToString(reader["FirstName"]);
                    student.Id = Guid.Parse(Convert.ToString(reader["Id"]));
                    student.LastName = Convert.ToString(reader["LastName"]);
                    student.MiddleName = Convert.ToString(reader["MiddleName"]);
                    student.Suffix = Convert.ToString(reader["Suffix"]);

                    students.Add(student);
                }
            }

            return students;
        }

        public void Remove(Guid id)
        {
            var sql = @"
                DELETE FROM CoursesTaken
                WHERE Student = @Id;
                DELETE FROM Students
                WHERE Id = @Id;";

            var parameter = new SqlParameter("@Id", id);

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameter))
                command.ExecuteNonQuery();
        }

        public void Update(StudentModel student)
        {
            var sql = @"
                UPDATE Students
                SET
                    FirstName = @FirstName,
                    MiddleName = @MiddleName,
                    LastName = @LastName,
                    Suffix = @Suffix,
                    DateOfBirth = @DateOfBirth
                WHERE Id = @Id";

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
        }
    }
}
