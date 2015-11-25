using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;

namespace CourseCentral.Domain.Repositories
{
    public interface CourseRepository
    {
        CourseModel Find(Guid id);
        IEnumerable<CourseModel> FindAll();
        void Add(CourseModel student);
        void Update(CourseModel student);
        void Remove(Guid id);
    }
}
