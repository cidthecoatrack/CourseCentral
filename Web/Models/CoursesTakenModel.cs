using CourseCentral.Domain.Models;
using System.Collections.Generic;

namespace CourseCentral.Web.Models
{
    public class CoursesTakenModel
    {
        public IEnumerable<CourseTakenModel> CoursesTaken { get; set; }
    }
}