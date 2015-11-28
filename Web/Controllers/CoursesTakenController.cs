using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Web.Models;
using System;
using System.Web.Mvc;

namespace CourseCentral.Web.Controllers
{
    public class CoursesTakenController : Controller
    {
        private CourseTakenRepository courseTakenRepository;

        public CoursesTakenController(CourseTakenRepository courseTakenRepository)
        {
            this.courseTakenRepository = courseTakenRepository;
        }

        [HttpGet]
        public ViewResult Student(Guid studentId)
        {
            var model = new CoursesTakenModel();
            model.CoursesTaken = courseTakenRepository.FindCourses(studentId);
            return View(model);
        }

        [HttpGet]
        public ViewResult Course(Guid courseId)
        {
            var model = new CoursesTakenModel();
            model.CoursesTaken = courseTakenRepository.FindStudents(courseId);
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