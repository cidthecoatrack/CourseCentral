using CourseDomain.Models;
using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        public StudentModel Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StudentModel> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(StudentModel student)
        {
            throw new NotImplementedException();
        }
    }
}
