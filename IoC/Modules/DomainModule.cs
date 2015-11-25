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
            Bind<CourseRepository>().ToMethod(c => new SqlServerCourseRepository(GetConnectionString()));
            Bind<CourseTakenRepository>().ToMethod(c => new SqlServerCourseTakenRepository(GetConnectionString()));
        }

        private String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["connectionString"];
        }
    }
}
