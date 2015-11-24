using CourseDomain.Models;
using System;
using System.Collections.Generic;

namespace CourseCentral.Domain.Repositories
{
    public interface StudentRepository
    {
        StudentModel Find(Guid id);
        IEnumerable<StudentModel> FindAll();
        void Add(StudentModel student);
        void Save(StudentModel student);
        void Remove(Guid id);
    }
}
