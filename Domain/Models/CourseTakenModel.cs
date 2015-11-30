using System;

namespace CourseCentral.Domain.Models
{
    public class CourseTakenModel
    {
        public NameModel Student { get; set; }
        public NameModel Course { get; set; }
        public Int32 Grade { get; set; }

        public CourseTakenModel()
        {
            Student = new NameModel();
            Course = new NameModel();
        }
    }
}
