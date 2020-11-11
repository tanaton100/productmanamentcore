using System.Collections.Generic;
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
        Task<int> AddAsync(Products entity);
        Task<int> UpdateAsync(Products entity);
        Task<int> DeleteAsync(int id);
    }

    public class ProductRepository : GenericReposiory<Products>,IProductRepository
    {

        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
           
        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Orders] ";
        }



        public override async Task< int> AddAsync(Products entity)
        {
            const string sqlCommand = @"INSERT INTO [PRODUCT] ([Name],[Price]) VALUES (@Name,@Price)SELECT CAST(SCOPE_IDENTITY() as int)";
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteScalarAsync<int>(sqlCommand, new
                {
                    entity.Id,
                    entity.Name,
                    entity.Price
                });
            });
        }

        public override async Task<int> UpdateAsync(Products entity)
        {
            var sqlCommand = @"UPDATE [PRODUCT] SET [Name] = @Name ,[Price] = @Price where [Id] =@Id";
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteAsync(sqlCommand, new
                {
                    entity.Id,
                    entity.Name,
                    entity.Price
                });
            });
        }

        public override async Task<int> DeleteAsync(int id)
        {
            var sqlCommand = @"DELETE FROM [PRODUCT] WHERE [Id] = @Id";
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteAsync(sqlCommand, new { Id = id });
            });
        }
    }
}