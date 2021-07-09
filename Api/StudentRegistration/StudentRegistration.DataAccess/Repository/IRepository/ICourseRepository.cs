using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.DataAccess.Repository.IRepository
{
    public interface ICourseRepository
    {
        Course Find(int id);
        List<Course> GetAll();

        Course Add(Course course);
        Task<Course> AddAsync(Course course);
        Course Update(Course course);

        void Remove(int id);
    }
}
