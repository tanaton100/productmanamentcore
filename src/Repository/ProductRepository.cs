using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Repository
{
    public interface IProductRepository
    {
        ValueTask<IEnumerable<Products>> GetAll();
        ValueTask<Products> FindById(int id);
        ValueTask<int> AddAsync(Products entity);
        ValueTask<int> UpdateAsync(Products entity);
        ValueTask<int> DeleteAsync(int id);
        ValueTask<IEnumerable<Products>> QueryBy(Expression<Func<Products, bool>> predicate);
    }

    public class ProductRepository : GenericReposiory<Products>,IProductRepository
    {

        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
           
        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Products] ";
        }



        public override async ValueTask< int> AddAsync(Products entity)
        {
            const string sqlCommand = @"INSERT INTO [Products] ([Name],[Price]) VALUES (@Name,@Price)SELECT CAST(SCOPE_IDENTITY() as int)";
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

        public override async ValueTask<int> UpdateAsync(Products entity)
        {
            var sqlCommand = @"UPDATE [Products] SET [Name] = @Name ,[Price] = @Price where [Id] =@Id";
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

        public override async ValueTask<int> DeleteAsync(int id)
        {
            var sqlCommand = @"DELETE FROM [Products] WHERE [Id] = @Id";
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteAsync(sqlCommand, new { Id = id });
            });
        }

        public override async ValueTask<IEnumerable<Products>> QueryBy(Expression<Func<Products, bool>> predicate)
        {


            return await WithConnection(async conn =>
            {
                return (await conn.QueryAsync<Products>(CreateSeleteString(), predicate));
            });
        }
    }
}