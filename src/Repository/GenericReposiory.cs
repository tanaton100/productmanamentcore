using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace ProductmanagementCore.Repository
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        ValueTask<TModel> FindById(int id);
        ValueTask<int> AddAsync(TModel entity);
        ValueTask<int> UpdateAsync(TModel entity);
        ValueTask<int> DeleteAsync(int id);
        Task<IEnumerable<TModel>> GetAll();
        ValueTask<IQueryable<TModel>> QueryBy(Func<TModel, bool> predicate);
    }

    public abstract class GenericReposiory<TModel> : IGenericRepository<TModel>
        where TModel : class, new()
    {
        private readonly string _connectionString;

        protected GenericReposiory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected async ValueTask<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return await getData(connection);
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
        }

       

        // use for buffered queries that do not return a type
        protected async Task WithConnection(Func<IDbConnection, Task> getData)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                await getData(connection);
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
        }

        //use for non-buffered queries that return a type
        protected async ValueTask<TResult> WithConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var data = await getData(connection);
                return await process(data);
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
        }


        public async Task<IEnumerable<TModel>> GetAll()
        {
            return await WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<TModel>(CreateSeleteString());
                return query;
            });


        }

        public async ValueTask<TModel> FindById(int id)
        {

            return await WithConnection(async conn =>
            {
                var query = await conn.QueryFirstOrDefaultAsync<TModel>(CreateSeleteString() + " WHERE Id = @Id", new { Id = id });
                return query;
            });
        }

        public abstract string CreateSeleteString();
        public abstract ValueTask<int> UpdateAsync(TModel tModel);
        public abstract ValueTask<int> DeleteAsync(int id);
        public abstract ValueTask<int> AddAsync(TModel tModel);
        public abstract ValueTask<IQueryable<TModel>> QueryBy(Func<TModel, bool> predicate);
    }

}
