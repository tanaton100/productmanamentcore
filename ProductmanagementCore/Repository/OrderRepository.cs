using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace ProductmanagementCore.Repository
{
    public interface IOrdersRepository
    {
        IEnumerable<Orders> GetAll();
        Orders FindById(int id);
        int Delete(int entity);
        int Update(Orders entity);
        int Add(Orders tModel);
        IEnumerable<Orders> FindByUserId(int id);
    }

    public class OrdersRepository : GenericReposiory<Orders>, IOrdersRepository
    {
        public OrdersRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Orders] ";
        }

        public override int Delete(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [Orders] WHERE [Id] = @Id");
            return this.DbConnection.Execute(sqlCommand, new
            {
                id

            });
        }
        public IEnumerable<Orders> FindByUserId(int id)
        {
            var sqlCommand = string.Format(@"SELECT * FROM [Orders] WHERE [IdUser] = @IdUser");
            return this.DbConnection.Query<Orders>(sqlCommand, new
            {
                IdUser = id
            }).ToList();
        }
        public override int Update(Orders entity)
        {
            var sqlCommand = string.Format(@"UPDATE [Orders] SET [IdProduct] = @IdProduct ,[IdUser] = @IdUser where [Id] =@Id");
            return this.DbConnection.Execute(sqlCommand, new
            {
                entity.Id,
                entity.IdProduct,
                entity.IdUser
            });
        }

        public override int Add(Orders tModel)
        {
            var sqlCommand = @"INSERT INTO [Orders] ([IdProduct],[IdUser]) VALUES (@IdProduct,@IdUser)SELECT CAST(SCOPE_IDENTITY() as int)";

            var resut = DbConnection.ExecuteScalar<int>(sqlCommand, new
            {
                tModel.Id,
                tModel.IdProduct,
                tModel.IdUser
            });
            tModel.Id = resut;
            return resut;
        }
    }
}
