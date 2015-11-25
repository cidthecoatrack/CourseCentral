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
                new SqlParameter("@Student", courseTaken.Student),
                new SqlParameter("@Course", courseTaken.Course),
                new SqlParameter("@Grade", courseTaken.Grade)
            };

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameters))
                command.ExecuteNonQuery();
        }

        public CourseTakenModel Find(Guid student, Guid course)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CourseTakenModel> FindCourses(Guid student)
        {
            var sql = @"SELECT * FROM CoursesTaken
                        WHERE Student = @Student";

            var coursesTaken = new List<CourseTakenModel>();
            var parameter = new SqlParameter("@Student", student);

            using (var connection = GetAndOpenConnection())
            using (var command = GetCommand(sql, connection, parameter))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var courseTaken = new CourseTakenModel();
                    courseTaken.Student = Guid.Parse(Convert.ToString(reader["Student"]));
                    courseTaken.Course = Guid.Parse(Convert.ToString(reader["Course"]));
                    courseTaken.Grade = Convert.ToInt32(reader["Grade"]);

                    coursesTaken.Add(courseTaken);
                }
            }

            return coursesTaken;
        }

        public IEnumerable<CourseTakenModel> FindStudents(Guid course)
        {
            throw new NotImplementedException();
        }

        public void Remove(CourseTakenModel courseTaken)
        {
            throw new NotImplementedException();
        }

        public void Update(CourseTakenModel courseTaken)
        {
            throw new NotImplementedException();
        }
    }
}
