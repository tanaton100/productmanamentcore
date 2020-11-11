using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace ProductmanagementCore.Repository
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Orders>> GetAll();
        Task<Orders> FindById(int id);
        Task<int> DeleteAsync(int entity);
        Task<int> UpdateAsync(Orders entity);
        Task<int> AddAsync(Orders tModel);
        Task<IEnumerable<Orders>> FindByUserId(int id);
    }

    public class OrdersRepository : GenericReposiory<Orders>, IOrdersRepository
    {
        private readonly IConfiguration _configuration;
        public OrdersRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Orders] ";
        }

        public override async Task<int> DeleteAsync(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [Orders] WHERE [Id] = @Id");
            return await DbConnection.ExecuteAsync(sqlCommand, new
            {
                id

            });
        }
        public async Task<IEnumerable<Orders>> FindByUserId(int id)
        {
            var sqlCommand = string.Format(@"SELECT * FROM [Orders] WHERE [IdUser] = @IdUser");
            return await DbConnection.QueryAsync<Orders>(sqlCommand, new
            {
                IdUser = id
            });
        }
        public override async Task<int> UpdateAsync(Orders entity)
        {
            var sqlCommand = string.Format(@"UPDATE [Orders] SET [IdProduct] = @IdProduct ,[IdUser] = @IdUser where [Id] =@Id");
            return await DbConnection.ExecuteAsync(sqlCommand, new
            {
                entity.Id,
                entity.ProductId,
                entity.UserId
            });
        }

        public override async Task<int> AddAsync(Orders tModel)
        {
            var sqlCommand = @"INSERT INTO [Orders] ([IdProduct],[IdUser]) VALUES (@IdProduct,@IdUser)SELECT CAST(SCOPE_IDENTITY() as int)";

            var resut = await DbConnection.ExecuteScalarAsync<int>(sqlCommand, new
            {
                tModel.Id,
                tModel.ProductId,
                tModel.UserId
            });
            tModel.Id = resut;
            return resut;
        }
    }
}
