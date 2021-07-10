using Dapper;
using Microsoft.Extensions.Configuration;
using StudentRegistration.DataAccess.Repository.Interfaces;
using StudentRegistration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using StudentRegistration.Model;

namespace StudentRegistration.DataAccess.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private IDbConnection db;
        public StudentRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public  async Task<int> AddAsync(Student entity)
        {
            using var transection = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
                try
                {
                    var sql = "INSERT INTO Students (Name, RegNo, Gender, DateOfBirth) VALUES(@Name, @RegNo, @Gender, @DateOfBirth);" +
                                             "SELECT CAST(SCOPE_IDENTITY() as int); ";
                    var id = await db.QueryAsync<int>(sql, entity);
                    entity.StudentId = id.FirstOrDefault();

                List<StudentCourse> studentCourses = new List<StudentCourse>();

                foreach (var course in entity.CourseList)
                {
                    studentCourses.Add(new StudentCourse { CourseId = course.CourseId, StudentId = entity.StudentId });
                }
                var sql1 = "INSERT INTO StudentCourse (StudentId, CourseId) VALUES(@StudentId, @CourseId);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int);";

               await db.ExecuteAsync(sql1, studentCourses);

                transection.Complete();

                return entity.StudentId;
                
                }
                catch (Exception ex)
                {
                    throw ex;
                 
                }
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool IfStudentExists(int regNo)
        {
          var result =  db.Query<Student>("select * from Students where RegNo = @regNo", new { regNo }).ToList();
          if(result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<int> UpdateAsync(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}
