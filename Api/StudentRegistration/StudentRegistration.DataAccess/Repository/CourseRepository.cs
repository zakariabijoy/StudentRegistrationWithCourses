using Dapper;
using Microsoft.Extensions.Configuration;
using StudentRegistration.DataAccess.Repository.Interfaces;
using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.DataAccess.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private IDbConnection db;
        public CourseRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public Task<int> AddAsync(Course entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Course>> GetAllAsync()
        {
            var result = await db.QueryAsync<Course>("usp_GetALLCourse", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public Task<Course> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Course>> GetByIdListAsync(List<int> id)
        {
            var result = await  db.QueryAsync<Course>("select * from Courses where CourseId in @id",new {id });
            return result.ToList();
        }

        public Task<int> UpdateAsync(Course entity)
        {
            throw new NotImplementedException();
        }
    }
}
