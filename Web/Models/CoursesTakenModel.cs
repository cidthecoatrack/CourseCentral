using CourseCentral.Domain.Models;
using System.Collections.Generic;

namespace CourseCentral.Web.Models
{
    public class CoursesTakenModel
    {
        public IEnumerable<CourseTakenModel> CoursesTaken { get; set; }
        public IEnumerable<NameModel> ToAssign { get; set; }
        public NameModel Student { get; set; }
        public NameModel Course { get; set; }
    }
}