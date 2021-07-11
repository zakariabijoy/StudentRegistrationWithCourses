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
using StudentRegistration.Utility.Helper;

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

        public async Task<int> DeleteAsync(int id)
        {
            using var transection = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var sql = @"delete from StudentCourse where StudentId = @id;
                            delete from Students where StudentId = @id;";
             var deletedId = await  db.ExecuteAsync(sql, new { id });
            transection.Complete();
            if(deletedId > 0)
            {
                return deletedId;
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<Student>> GetAllAsync()
        {
            var sql = @"select s.StudentId, s.Name, s.RegNo, s.Gender, s.DateOfBirth, c.CourseId, c.Name, c.Credit
                        from Students s
                        inner join
                        StudentCourse sc on sc.StudentId = s.StudentId
                        inner join 
                        Courses c on c.CourseId = sc.CourseId;";

            var studentDic = new Dictionary<int, Student>(); 

            var students = await db.QueryAsync<Student, Course, Student>(sql, (s, c) => {         

                if (!studentDic.TryGetValue(s.StudentId, out var currentStudent))
                {
                    currentStudent = s;
                    studentDic.Add(currentStudent.StudentId, currentStudent);
                }

                currentStudent.CourseList.Add(c);
                return currentStudent;
            }, splitOn: "CourseId");

            return students.Distinct().ToList();
        }

        public async Task<PagedList<Student>> GetAllAsyncWithPaginationAsync(StudentsParams studentsParams)
        {
            var sql = @"select s.StudentId, s.Name, s.RegNo, s.Gender, s.DateOfBirth, c.CourseId, c.Name, c.Credit
                        from Students s
                        inner join
                        StudentCourse sc on sc.StudentId = s.StudentId
                        inner join 
                        Courses c on c.CourseId = sc.CourseId;";

            var studentDic = new Dictionary<int, Student>();

            var students = await db.QueryAsync<Student, Course, Student>(sql, (s, c) => {

                if (!studentDic.TryGetValue(s.StudentId, out var currentStudent))
                {
                    currentStudent = s;
                    studentDic.Add(currentStudent.StudentId, currentStudent);
                }

                currentStudent.CourseList.Add(c);
                return currentStudent;
            }, splitOn: "CourseId");

            return PagedList<Student>.Create(students.Distinct().AsQueryable(),studentsParams.PageNumber, studentsParams.pageSize);
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            var sql = @"select s.StudentId, s.Name, s.RegNo, s.Gender, s.DateOfBirth, c.CourseId, c.Name, c.Credit
                        from Students s
                        inner join
                        StudentCourse sc on sc.StudentId = s.StudentId
                        inner join 
                        Courses c on c.CourseId = sc.CourseId
                        where s.StudentId = @id;";

            var studentDic = new Dictionary<int, Student>();

            var students = await db.QueryAsync<Student, Course, Student>(sql, (s, c) => {

                if (!studentDic.TryGetValue(s.StudentId, out var currentStudent))
                {
                    currentStudent = s;
                    studentDic.Add(currentStudent.StudentId, currentStudent);
                }

                currentStudent.CourseList.Add(c);
                return currentStudent;
            },new {id }, splitOn: "CourseId");

            return students.Distinct().FirstOrDefault();
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
