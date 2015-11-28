using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;

namespace CourseCentral.Domain.Repositories
{
    public interface CourseRepository
    {
        IEnumerable<CourseModel> FindAll();
        void Add(CourseModel course);
        void Update(CourseModel course);
        void Remove(Guid id);
    }
}
