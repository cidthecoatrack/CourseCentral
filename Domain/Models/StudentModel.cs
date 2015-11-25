using System;

namespace CourseCentral.Domain.Models
{
    public class StudentModel
    {
        public Guid Id { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String Suffix { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
