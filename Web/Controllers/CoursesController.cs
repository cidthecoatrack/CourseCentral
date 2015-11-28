﻿using CourseCentral.Domain.Models;
using CourseCentral.Domain.Repositories;
using CourseCentral.Domain.Services;
using System;
using System.Web.Mvc;

namespace CourseCentral.Web.Controllers
{
    public class CoursesController : Controller
    {
        private CourseRepository courseRepository;
        private CourseService courseService;

        public CoursesController(CourseRepository courseRepository, CourseService courseService)
        {
            this.courseRepository = courseRepository;
            this.courseService = courseService;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult FindAll()
        {
            var courses = courseRepository.FindAll();
            return Json(new { courses = courses }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Update(CourseModel course)
        {
            courseRepository.Update(course);
        }

        [HttpPost]
        public void Add(CourseModel course)
        {
            courseRepository.Add(course);
        }

        [HttpPost]
        public void Remove(Guid studentId)
        {
            courseService.Remove(studentId);
        }
    }
}