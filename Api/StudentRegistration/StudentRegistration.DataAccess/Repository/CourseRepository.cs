using Dapper;
using Microsoft.Extensions.Configuration;
using StudentRegistration.DataAccess.Repository.IRepository;
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
        public Course Add(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<Course> AddAsync(Course course)
        {
            throw new NotImplementedException();
        }

        public Course Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<Course> GetAll()
        {
            var sql = "SELECT * FROM Courses";
            return db.Query<Course>(sql).ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Course Update(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
