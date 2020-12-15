using Microsoft.Extensions.Configuration;
using ProductmanagementCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace ProductmanagementCore.Repository
{
    public interface IOrdersRepository
    {
        ValueTask<IEnumerable<Orders>> GetAll();
        ValueTask<Orders> FindById(int id);
        ValueTask<int> DeleteAsync(int entity);
        ValueTask<int> UpdateAsync(Orders entity);
        ValueTask<int> AddAsync(Orders entity);
        ValueTask<IEnumerable<Orders>> FindByUserId(int id);
        ValueTask<IQueryable<Orders>> QueryBy(Expression<Func<Orders, bool>> predicate);

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

        public override async ValueTask<int> DeleteAsync(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [Orders] WHERE [Id] = @Id");
            return await WithConnection(async conn =>
             {
                 return await conn.ExecuteAsync(sqlCommand, new { Id = id });
             });
        }
        public async ValueTask<IEnumerable<Orders>> FindByUserId(int id)
        {
            var sqlCommand = string.Format(@"SELECT * FROM [Orders] WHERE [UserId] = @UserId");

            return await WithConnection(async conn =>
            {
                return await conn.QueryAsync<Orders>(sqlCommand, new { Id = id });
            });
        }
        public override async ValueTask<int> UpdateAsync(Orders entity)
        {
            var sqlCommand = string.Format(@"UPDATE [Orders] SET [ProductId] = @ProductId ,[UserId] = @UserId where [Id] =@Id");
            return await WithConnection(async conn =>
            {
                return await conn.ExecuteAsync(sqlCommand, new
                {
                    entity.Id,
                    entity.ProductId,
                    entity.UserId
                });
            });
        }

        public override async ValueTask<int> AddAsync(Orders entity)
        {
            var sqlCommand = @"INSERT INTO [Orders] ([ProductId],[UserId]) VALUES (@ProductId,@UserId)SELECT CAST(SCOPE_IDENTITY() as int)";

            return await WithConnection(async conn =>
            {
                return await conn.ExecuteScalarAsync<int>(sqlCommand, new
                {
                    entity.Id,
                    entity.ProductId,
                    entity.UserId
                });
            });
        }

        public override ValueTask<IQueryable<Orders>> QueryBy(Expression<System.Func<Orders, bool>> predicate)
        {
            throw new System.NotImplementedException();
        }
    }
}
