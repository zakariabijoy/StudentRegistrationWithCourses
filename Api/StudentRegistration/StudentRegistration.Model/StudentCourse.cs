using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Model
{
    public class StudentCourse
    {
        public int StudentCourseId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
