using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Repositories.Domain;
using CourseCentral.Domain.Services;
using CourseCentral.Domain.Services.Domain;
using Ninject.Modules;
using System;
using System.Configuration;
using Tree;
using Tree.Domain;

namespace CourseCentral.IoC.Modules
{
    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<StudentRepository>().ToMethod(c => new SqlServerStudentRepository(GetConnectionString()));
            Bind<CourseRepository>().ToMethod(c => new SqlServerCourseRepository(GetConnectionString()));
            Bind<CourseTakenRepository>().ToMethod(c => new SqlServerCourseTakenRepository(GetConnectionString()));
            Bind<StudentService>().To<DomainStudentService>();
            Bind<CourseService>().To<DomainCourseService>();
        }

        private String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["connectionString"];
        }
    }
}
