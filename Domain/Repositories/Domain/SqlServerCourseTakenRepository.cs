using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CourseCentral.Domain.Repositories.Domain
{
    public class SqlServerCourseTakenRepository : Repository, CourseTakenRepository
    {
        public SqlServerCourseTakenRepository(String connectionString)
            : base(connectionString)
        {
        }

        public void Add(CourseTakenModel courseTaken)
        {
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
        }

        public IEnumerable<CourseTakenModel> FindCourses(Guid student)
        {
            var sql = @"SELECT ct.Course, ct.Student, ct.Grade, s.FirstName, c.Name
                        FROM CoursesTaken ct
                        INNER JOIN Courses c ON ct.Course = c.Id
                        INNER JOIN Students s ON ct.Student = s.Id
                        WHERE ct.Student = @Student";

            var coursesTaken = new List<CourseTakenModel>();
            var parameter = new SqlParameter("@Student", student);

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameter))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var courseTaken = new CourseTakenModel();
                    courseTaken.Student.Id = Guid.Parse(Convert.ToString(reader["Student"]));
                    courseTaken.Student.Name = Convert.ToString(reader["FirstName"]);
                    courseTaken.Course.Id = Guid.Parse(Convert.ToString(reader["Course"]));
                    courseTaken.Course.Name = Convert.ToString(reader["Name"]);
                    courseTaken.Grade = Convert.ToInt32(reader["Grade"]);

                    coursesTaken.Add(courseTaken);
                }
            }

            return coursesTaken;
        }

        public IEnumerable<CourseTakenModel> FindStudents(Guid course)
        {
            var sql = @"SELECT ct.Course, ct.Student, ct.Grade, s.FirstName, c.Name
                        FROM CoursesTaken ct
                        INNER JOIN Courses c ON ct.Course = c.Id
                        INNER JOIN Students s ON ct.Student = s.Id
                        WHERE ct.Course = @Course";

            var coursesTaken = new List<CourseTakenModel>();
            var parameter = new SqlParameter("@Course", course);

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameter))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var courseTaken = new CourseTakenModel();
                    courseTaken.Student.Id = Guid.Parse(Convert.ToString(reader["Student"]));
                    courseTaken.Student.Name = Convert.ToString(reader["FirstName"]);
                    courseTaken.Course.Id = Guid.Parse(Convert.ToString(reader["Course"]));
                    courseTaken.Course.Name = Convert.ToString(reader["Name"]);
                    courseTaken.Grade = Convert.ToInt32(reader["Grade"]);

                    coursesTaken.Add(courseTaken);
                }
            }

            return coursesTaken;
        }

        public void Remove(CourseTakenModel courseTaken)
        {
            var sql = @"
                DELETE FROM CoursesTaken
                WHERE Student = @Student
                    AND Course = @Course";

            var parameters = new[]
            {
                new SqlParameter("@Student", courseTaken.Student.Id),
                new SqlParameter("@Course", courseTaken.Course.Id)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();
        }

        public void Update(CourseTakenModel courseTaken)
        {
            var sql = @"
                UPDATE CoursesTaken
                SET Grade = @Grade
                WHERE Student = @Student
                    AND Course = @Course";

            var parameters = new[]
            {
                new SqlParameter("@Student", courseTaken.Student.Id),
                new SqlParameter("@Course", courseTaken.Course.Id),
                new SqlParameter("@Grade", courseTaken.Grade)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();
        }
    }
}
