using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;

namespace ProductmanagementCore.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Products> GetAll();
        Products FindBy(int id);
        int Add(Products entity);
        int Update(Products entity);
        int Delete(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private IDbConnection Db { get; set; }

        public ProductRepository(IConfiguration configuration)
        {
            var connectionstring = configuration.GetSection("MsSqlConnectionString");
            Db = new SqlConnection(connectionstring.Value);
        }

        public IEnumerable<Products> GetAll()
        {
            return Db.Query<Products>("Select * From [PRODUCT] ").ToList();
        }

        public Products FindBy(int id)
        {
            var sqlCommand = @"SELECT * FROM [PRODUCT] WHERE[Id] = @Id";
            return this.Db.Query<Products>(sqlCommand, new
            {
                id
            }).FirstOrDefault();
        }
        public int Add(Products entity)
        {
            const string sqlCommand = @"INSERT INTO [PRODUCT] ([Name],[Price]) VALUES (@Name,@Price)SELECT CAST(SCOPE_IDENTITY() as int)";
            return this.Db.ExecuteScalar<int>(sqlCommand, new
            {
                entity.Id,
                entity.Name,
                entity.Price
            });
        }

        public int Update(Products entity)
        {
            var sqlCommand = @"UPDATE [PRODUCT] SET [Name] = @Name ,[Price] = @Price where [Id] =@Id";
            return this.Db.Execute(sqlCommand, new
            {
                entity.Id,
                entity.Name,
                entity.Price
            });
        }

        public int Delete(int id)
        {
            var sqlCommand = @"DELETE FROM [PRODUCT] WHERE [Id] = @Id";
            return this.Db.Execute(sqlCommand, new
            {
                id
            });
        }
    }
}