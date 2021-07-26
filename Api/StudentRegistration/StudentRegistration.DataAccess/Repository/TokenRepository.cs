using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Microsoft.Extensions.Configuration;
using StudentRegistration.DataAccess.Repository.Interfaces;
using StudentRegistration.Model;

namespace StudentRegistration.DataAccess.Repository
{
    public class TokenRepository : ITokenRepositoy
    {
        private IDbConnection db;
        public TokenRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<int> AddAsync(Token entity)
        {
            entity.LastModifiedDate = DateTime.Now;
            var sql = "INSERT INTO Tokens (ClientId, Value, CreatedDate, UserId, LastModifiedDate, ExpiryTime) VALUES(@ClientId, @Value, @CreatedDate, @UserId, @LastModifiedDate, @ExpiryTime); ";
            return await db.ExecuteAsync(sql, entity);
 
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var transection = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var sql = @"delete from Tokens where UserId = @id;";
                            
            var deletedId = await db.ExecuteAsync(sql, new { id });
            transection.Complete();
            if (deletedId > 0)
            {
                return deletedId;
            }
            else
            {
                return 0;
            }
        }

        public Task<List<Token>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Token> GetByClientIdAndRefreshtokenAsync(string clientId, string refreshToken)
        {
            var result = db.Query<Token>("select * from Tokens where ClientId = @clientId and Value= @refreshToken", new { @clientId= clientId, @refreshToken = refreshToken }).FirstOrDefault();
            return result;
        }

        public Task<Token> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Token entity)
        {
            throw new NotImplementedException();
        }
    }
}
