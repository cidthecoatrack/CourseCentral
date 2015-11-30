using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CourseCentral.Web.Controllers
{
    public class CoursesTakenController : Controller
    {
        private CourseTakenRepository courseTakenRepository;
        private CourseRepository courseRepository;
        private StudentRepository studentRepository;

        public CoursesTakenController(CourseTakenRepository courseTakenRepository, CourseRepository courseRepository, StudentRepository studentRepository)
        {
            this.courseTakenRepository = courseTakenRepository;
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
        }

        [HttpGet]
        public ViewResult Student(Guid id)
        {
            var model = new CoursesTakenModel();
            model.CoursesTaken = courseTakenRepository.FindCourses(id);

            var courses = courseRepository.FindAll();
            model.ToAssign = courses.Select(c => new NameModel { Id = c.Id, Name = c.Name });

            var student = studentRepository.FindAll().First(s => s.Id == id);
            model.Student = new NameModel { Id = id, Name = student.FirstName };

            return View(model);
        }

        [HttpGet]
        public ViewResult Course(Guid id)
        {
            var model = new CoursesTakenModel();
            model.CoursesTaken = courseTakenRepository.FindStudents(id);

            var students = studentRepository.FindAll();
            model.ToAssign = students.Select(s => new NameModel { Id = s.Id, Name = s.FirstName });

            var course = courseRepository.FindAll().First(s => s.Id == id);
            model.Course = new NameModel { Id = id, Name = course.Name };

            return View(model);
        }

        [HttpPost]
        public void Update(CourseTakenModel courseTaken)
        {
            courseTakenRepository.Update(courseTaken);
        }

        [HttpPost]
        public void Add(CourseTakenModel courseTaken)
        {
            courseTakenRepository.Add(courseTaken);
        }

        [HttpPost]
        public void Remove(CourseTakenModel courseTaken)
        {
            courseTakenRepository.Remove(courseTaken);
        }
    }
}