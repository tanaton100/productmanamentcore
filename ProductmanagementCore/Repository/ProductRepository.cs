using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAll();
        Task<Products> FindById(int id);
        Task<int> Add(Products entity);
        Task<int> Update(Products entity);
        Task<int> Delete(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private IDbConnection Db { get; set; }

        public ProductRepository(IConfiguration configuration)
        {
            var connectionstring = configuration.GetSection("MsSqlConnectionString");
            Db = new SqlConnection(connectionstring.Value);
        }

        public async Task< IEnumerable<Products>> GetAll()
        {
            return await Db.QueryAsync<Products>("Select * From [PRODUCT] ");
        }

        public async Task<Products> FindById(int id)
        {
            var sqlCommand = @"SELECT * FROM [PRODUCT] WHERE[Id] = @Id";
            return await Db.QueryFirstOrDefaultAsync<Products>(sqlCommand, new
            {
                id
            });
        }
        public async Task< int> Add(Products entity)
        {
            const string sqlCommand = @"INSERT INTO [PRODUCT] ([Name],[Price]) VALUES (@Name,@Price)SELECT CAST(SCOPE_IDENTITY() as int)";
            return await Db.ExecuteScalarAsync<int>(sqlCommand, new
            {
                entity.Id,
                entity.Name,
                entity.Price
            });
        }

        public async Task< int> Update(Products entity)
        {
            var sqlCommand = @"UPDATE [PRODUCT] SET [Name] = @Name ,[Price] = @Price where [Id] =@Id";
            return await Db.ExecuteAsync(sqlCommand, new
            {
                entity.Id,
                entity.Name,
                entity.Price
            });
        }

        public async Task<int> Delete(int id)
        {
            var sqlCommand = @"DELETE FROM [PRODUCT] WHERE [Id] = @Id";
            return await Db.ExecuteAsync(sqlCommand, new
            {
                id
            });
        }
    }
}