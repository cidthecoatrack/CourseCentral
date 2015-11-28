using CourseCentral.Domain.Models;
using System.Collections.Generic;

namespace CourseCentral.Web.Models
{
    public class StudentsModel
    {
        public IEnumerable<StudentModel> Students { get; set; }
    }
}