using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Repository
{
    public interface IUserRepository
    {
        int Add(Users tModel);
        int Delete(int id);
        IEnumerable<Users> GetAll();
        Users FindById(int id);
        int Update(Users tUsers);
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

        public override int Add(Users tModel)
        {
            var sqlCommand = @"INSERT INTO [Users] ([Username],[Fristname],[Lastname],[Email],[Tel],[Password])
                            VALUES (@Username,@Fristname,@Lastname,@Email,@Tel,@Password)SELECT CAST(SCOPE_IDENTITY() as int)";

            var resut = DbConnection.ExecuteScalar<int>(sqlCommand, new
            {
                tModel.Id,
                tModel.Email,
                tModel.Fristname,
                tModel.Username,
                tModel.Lastname,
                tModel.Tel,
                tModel.Password
            });
            tModel.Id = resut;
            return resut;
        }

        public override int Delete(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [Users] WHERE [Id] = @Id");
            return this.DbConnection.Execute(sqlCommand, new
            {
                 id
            });
        }

        public override int Update(Users tModel)
        {
            var sqlCommand = string.Format(@"UPDATE [Users] SET [Username] = @Username ,[Fristname] = @Fristname ,[Lastname] =@Lastname ,[Email] =@Email ,[Tel] =@Tel,[Password] = @Password WHERE [Id] = @Id");
            return this.DbConnection.Execute(sqlCommand, new
            {
                tModel.Id,
                tModel.Username,
                tModel.Fristname,
                tModel.Lastname,
                tModel.Email,
                tModel.Tel,
                tModel.Password
            });
        }
    }
}
