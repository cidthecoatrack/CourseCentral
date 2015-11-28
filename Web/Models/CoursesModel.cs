using CourseCentral.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseCentral.Web.Models
{
    public class CoursesModel
    {
        public IEnumerable<CourseModel> Courses { get; set; }
    }
}