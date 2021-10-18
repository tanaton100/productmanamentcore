using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Common;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAll();
        ValueTask<Products> FindById(int id);
        ValueTask<int> AddAsync(Products entity);
        ValueTask<int> UpdateAsync(Products entity);
        ValueTask<int> DeleteAsync(int id);
        ValueTask<IQueryable<Products>> QueryBy(Func<Products, bool> predicate);
        Task UpdateMutiProductWithStored(List<Products> products);
    }

    public class ProductRepository : GenericReposiory<Products>, IProductRepository
    {

        public ProductRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Products] ";
        }



        public override async ValueTask<int> AddAsync(Products entity)
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

        public override async ValueTask<IQueryable<Products>> QueryBy(Func<Products, bool> predicate)
        {


            return await WithConnection(async conn => (await conn.QueryAsync<Products>(CreateSeleteString(), null)).Where(predicate).AsQueryable());
        }

        public async Task UpdateMutiProductWithStored(List<Products> products)
        {
            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-5LVT2F6\\SQLEXPRESS;Database=demo;Integrated Security=True;MultipleActiveResultSets=True;"))
            {
                try
                {
                    await connection.OpenAsync();
                    var nameStored = "UpdateProductList";

                    var dataTableProduct = products.ToDataTable();
                    dataTableProduct.TableName = "products_Data";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@products_Data", dataTableProduct.AsTableValuedParameter("[dbo].[Product_type]"));
                    await connection.ExecuteAsync(nameStored, dynamicParameters, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}