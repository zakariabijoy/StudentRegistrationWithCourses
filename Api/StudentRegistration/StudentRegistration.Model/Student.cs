﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Model
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int RegNo { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public IList<Course> CourseList { get; set; }

    }
}
