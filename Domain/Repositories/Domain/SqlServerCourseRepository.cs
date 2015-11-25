using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;

namespace CourseCentral.Domain.Repositories.Domain
{
    public class SqlServerCourseRepository : Repository, CourseRepository
    {
        public SqlServerCourseRepository(String connectionString)
            : base(connectionString)
        {
        }

        public void Add(CourseModel student)
        {
            throw new NotImplementedException();
        }

        public CourseModel Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CourseModel> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(CourseModel student)
        {
            throw new NotImplementedException();
        }
    }
}
