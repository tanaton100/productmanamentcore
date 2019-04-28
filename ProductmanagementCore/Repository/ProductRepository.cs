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
        private IDbConnection _db { get; set; }

        public ProductRepository(IConfiguration configuration)
        {
            var connectionstring = configuration.GetSection("MsSqlConnectionString");
            _db = new SqlConnection("Data Source = GEOGERR; Initial Catalog = PRODUCTM; Integrated Security = true;");
        }

        public IEnumerable<Products> GetAll()
        {
            return _db.Query<Products>("Select * From [PRODUCT] ").ToList();
        }

        public Products FindBy(int id)
        {
            var sqlCommand = string.Format(@"SELECT * FROM [PRODUCT] WHERE[Id] = @Id");
            return this._db.Query<Products>(sqlCommand, new
            {
                id
            }).FirstOrDefault();
        }
        public int Add(Products entity)
        {
            var sqlCommand = @"INSERT INTO [PRODUCT] ([Name],[Price]) VALUES (@Name,@Price)SELECT CAST(SCOPE_IDENTITY() as int)";
            return this._db.ExecuteScalar<int>(sqlCommand, new
            {
                entity.Id,
                entity.Name,
                entity.Price
            });
        }

        public int Update(Products entity)
        {
            var sqlCommand = string.Format(@"UPDATE [PRODUCT] SET [Name] = @Name ,[Price] = @Price where [Id] =@Id");
            return this._db.Execute(sqlCommand, new
            {
                entity.Id,
                entity.Name,
                entity.Price
            });
        }

        public int Delete(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [PRODUCT] WHERE [Id] = @Id");
            return this._db.Execute(sqlCommand, new
            {
                id
            });
        }
    }
}