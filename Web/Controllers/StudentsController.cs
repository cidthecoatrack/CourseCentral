using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Services;
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
            return View();
        }

        [HttpGet]
        public JsonResult FindAll()
        {
            var students = studentRepository.FindAll();
            return Json(new { students = students }, JsonRequestBehavior.AllowGet);
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