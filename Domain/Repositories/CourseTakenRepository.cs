using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;

namespace CourseCentral.Domain.Repositories
{
    public interface CourseTakenRepository
    {
        IEnumerable<CourseTakenModel> FindCourses(Guid student);
        IEnumerable<CourseTakenModel> FindStudents(Guid course);
        CourseTakenModel Find(Guid student, Guid course);
        void Add(CourseTakenModel courseTaken);
        void Remove(CourseTakenModel courseTaken);
        void Update(CourseTakenModel courseTaken);
    }
}
