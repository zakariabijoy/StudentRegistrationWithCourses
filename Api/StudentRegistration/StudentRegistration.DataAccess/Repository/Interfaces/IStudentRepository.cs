using StudentRegistration.Model;
using StudentRegistration.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.DataAccess.Repository.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        bool IfStudentExists(int regNo);
        Task<PagedList<Student>> GetAllAsyncWithPaginationAsync(StudentsParams studentsParams);

    }
}
