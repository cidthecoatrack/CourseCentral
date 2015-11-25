using System;

namespace CourseCentral.Domain.Models
{
    public class CourseModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Department { get; set; }
        public Int32 Number { get; set; }
        public Char Section { get; set; }
        public String Professor { get; set; }
        public Int32 Year { get; set; }
        public String Semester { get; set; }
    }
}
