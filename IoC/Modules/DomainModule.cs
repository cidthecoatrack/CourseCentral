using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Repositories.Domain;
using Ninject.Modules;
using System;
using System.Configuration;

namespace CourseCentral.IoC.Modules
{
    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<StudentRepository>().ToMethod(c => new SqlServerStudentRepository(GetConnectionString()));
        }

        private String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["connectionString"];
        }
    }
}
