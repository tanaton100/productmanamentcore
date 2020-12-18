using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Repository
{
    public interface IUserRepository
    {
        ValueTask<int> AddAsync(Users tModel);
        ValueTask<int> DeleteAsync(int id);
        ValueTask<IEnumerable<Users>> GetAll();
        ValueTask<Users> FindById(int id);
        ValueTask<int> UpdateAsync(Users tUsers);
        ValueTask<IQueryable<Users>> QueryBy(Func<Users, bool> predicate);
    }
    public class UserRepository : GenericReposiory<Users>, IUserRepository
    {

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Users] ";
        }

        public override async ValueTask<int> AddAsync(Users tModel)
        {
            var sqlCommand = @"INSERT INTO [Users] ([Username],[Firstname],[Lastname],[Email],[Tel])
                            VALUES (@Username,@Firstname,@Lastname,@Email,@Tel)SELECT CAST(SCOPE_IDENTITY() as int)";

            return await WithConnection(async conn =>
           {
               return await conn.ExecuteScalarAsync<int>(sqlCommand, new
               {
                   tModel.Id,
                   tModel.Email,
                   tModel.Firstname,
                   tModel.Username,
                   tModel.Lastname,
                   tModel.Tel,
               });
           });


        }

        public override async ValueTask<int> DeleteAsync(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [Users] WHERE [Id] = @Id");
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteAsync(sqlCommand, new { Id = id });
            });

        }

        public override async ValueTask<int> UpdateAsync(Users tModel)
        {
            var sqlCommand = string.Format(@"UPDATE [Users] SET [Username] = @Username ,[Firstname] = @Firstname ,[Lastname] =@Lastname ,[Email] =@Email ,[Tel] =@Tel,[Password] = @Password WHERE [Id] = @Id");


            return await WithConnection(async conn =>
            {
                return await conn.ExecuteAsync(sqlCommand, new
                {
                    tModel.Id,
                    tModel.Username,
                    Fristname = tModel.Firstname,
                    tModel.Lastname,
                    tModel.Email,
                    tModel.Tel,
                });
            });
        }

        public override ValueTask<IQueryable<Users>> QueryBy(Func<Users, bool> predicate)
        {
            throw new System.NotImplementedException();
        }
    }
}
