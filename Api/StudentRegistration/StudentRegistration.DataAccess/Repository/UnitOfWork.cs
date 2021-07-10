using StudentRegistration.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ICourseRepository courseRepository, IStudentRepository studentRepository)
        {
            Courses = courseRepository;
            Students = studentRepository;
        }

        public ICourseRepository Courses { get; set; }
        public IStudentRepository Students { get; set; }
        
    }
}
