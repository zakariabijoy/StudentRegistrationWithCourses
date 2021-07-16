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
        public UnitOfWork(ICourseRepository courseRepository, IStudentRepository studentRepository,IUserRepository userRepository)
        {
            Courses = courseRepository;
            Students = studentRepository;
            Users = userRepository;
        }

        public ICourseRepository Courses { get; set; }
        public IStudentRepository Students { get; set; }
        public IUserRepository Users { get; set; }
        
    }
}
