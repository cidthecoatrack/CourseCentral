using System;

namespace CourseCentral.Domain.Models
{
    public class CourseTakenModel
    {
        public Guid Student { get; set; }
        public Guid Course { get; set; }
        public Int32 Grade { get; set; }
    }
}
