using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;

namespace CourseCentral.Domain.Repositories
{
    public interface StudentRepository
    {
        IEnumerable<StudentModel> FindAll();
        void Add(StudentModel student);
        void Update(StudentModel student);
        void Remove(Guid id);
    }
}
