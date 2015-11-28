using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CourseCentral.Domain.Repositories.Domain
{
    public class SqlServerCourseRepository : Repository, CourseRepository
    {
        public SqlServerCourseRepository(String connectionString)
            : base(connectionString)
        {
        }

        public void Add(CourseModel course)
        {
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
        }

        public IEnumerable<CourseModel> FindAll()
        {
            var sql = @"SELECT * FROM Courses";
            var courses = new List<CourseModel>();

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var course = new CourseModel();
                    course.Department = Convert.ToString(reader["Department"]);
                    course.Id = Guid.Parse(Convert.ToString(reader["Id"]));
                    course.Name = Convert.ToString(reader["Name"]);
                    course.Number = Convert.ToInt32(reader["Number"]);
                    course.Professor = Convert.ToString(reader["Professor"]);
                    course.Section = Convert.ToChar(reader["Section"]);
                    course.Semester = Convert.ToString(reader["Semester"]);
                    course.Year = Convert.ToInt32(reader["Year"]);

                    courses.Add(course);
                }
            }

            return courses;
        }

        public void Remove(Guid id)
        {
            var sql = @"
                DELETE FROM Courses
                WHERE Id = @Id";

            var parameter = new SqlParameter("@Id", id);

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameter))
                command.ExecuteNonQuery();
        }

        public void Update(CourseModel course)
        {
            var sql = @"
                UPDATE Courses
                SET
                    Name = @Name,
                    Department = @Department,
                    Number = @Number,
                    Section = @Section,
                    Professor = @Professor,
                    Year = @Year,
                    Semester = @Semester
                WHERE Id = @Id";

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
        }
    }
}
