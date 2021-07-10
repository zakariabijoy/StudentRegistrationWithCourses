using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Api.DTOs
{
    public class CourseCheckBoxDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Credit { get; set; }
        public bool Ischecked { get; set; } = false;
    }
}
