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
        public UnitOfWork(ICourseRepository courseRepository)
        {
            Courses = courseRepository;
        }

        public ICourseRepository Courses { get; set; }
    }
}
