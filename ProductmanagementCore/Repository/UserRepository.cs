using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Repository
{
    public interface IUserRepository
    {
        Task<int> AddAsync(Users tModel);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Users>> GetAll();
        Task<Users> FindById(int id);
        Task<int> UpdateAsync(Users tUsers);
    }
    public class UserRepository : GenericReposiory<Users>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Users] ";
        }

        public override async Task <int> AddAsync(Users tModel)
        {
            var sqlCommand = @"INSERT INTO [Users] ([Username],[Fristname],[Lastname],[Email],[Tel],[Password])
                            VALUES (@Username,@Fristname,@Lastname,@Email,@Tel,@Password)SELECT CAST(SCOPE_IDENTITY() as int)";

            var resut = await DbConnection.ExecuteScalarAsync<int>(sqlCommand, new
            {
                tModel.Id,
                tModel.Email,
                tModel.Firstname,
                tModel.Username,
                tModel.Lastname,
                tModel.Tel,
                tModel.Password
            });
            tModel.Id = resut;
            return resut;
        }

        public override async Task<int> DeleteAsync(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [Users] WHERE [Id] = @Id");
            return await DbConnection.ExecuteAsync(sqlCommand, new
            {
                id
            });
        }

        public override async Task<int> UpdateAsync(Users tModel)
        {
            var sqlCommand = string.Format(@"UPDATE [Users] SET [Username] = @Username ,[Fristname] = @Fristname ,[Lastname] =@Lastname ,[Email] =@Email ,[Tel] =@Tel,[Password] = @Password WHERE [Id] = @Id");
            return await DbConnection.ExecuteAsync(sqlCommand, new
            {
                tModel.Id,
                tModel.Username,
                Fristname = tModel.Firstname,
                tModel.Lastname,
                tModel.Email,
                tModel.Tel,
                tModel.Password
            });
        }
    }
}
