using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Services;
using CourseCentral.Web.Models;
using System;
using System.Web.Mvc;

namespace CourseCentral.Web.Controllers
{
    public class StudentsController : Controller
    {
        private StudentRepository studentRepository;
        private StudentService studentService;

        public StudentsController(StudentRepository studentRepository, StudentService studentService)
        {
            this.studentRepository = studentRepository;
            this.studentService = studentService;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var model = new StudentsModel();
            model.Students = studentRepository.FindAll();
            return View(model);
        }

        [HttpPost]
        public void Update(StudentModel student)
        {
            studentRepository.Update(student);
        }

        [HttpPost]
        public void Add(StudentModel student)
        {
            studentRepository.Add(student);
        }

        [HttpPost]
        public void Remove(Guid studentId)
        {
            studentService.Remove(studentId);
        }
    }
}