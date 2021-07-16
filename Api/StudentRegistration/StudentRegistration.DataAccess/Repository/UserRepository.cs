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
    public class UserRepository : IUserRepository
    {
        private IDbConnection db;
        public UserRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<int> AddAsync(User entity)
        {
            var sql = @"insert into Users(Name, PasswordHash, PasswordSalt) values (@Name, @PasswordHash, @PasswordSalt);                   
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await db.QueryAsync<int>(sql, entity);

            return id.FirstOrDefault();
        }

        public async Task<User> AddUserAsync(User entity)
        {
            var sql = @"insert into Users(Name, PasswordHash, PasswordSalt) values (@Name, @PasswordHash, @PasswordSalt);                   
                        SELECT * from Users WHERE Name = @Name";
            var u = await db.QueryAsync<User>(sql, entity);

            return u.FirstOrDefault();
        }


        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByNameAsync(string name)
        {
            var sql = "select * from Users  where Name = @Name";
            var user =  await db.QueryAsync<User>(sql, new { @Name = name });
            return user.FirstOrDefault();
        }

        public Task<int> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
