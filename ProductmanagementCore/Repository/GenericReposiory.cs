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
    public abstract class GenericReposiory<TModel>
    {
        public IDbConnection DbConnection { get; set; }


        protected GenericReposiory(IConfiguration configuration)
        {
            var connectionstring = configuration.GetSection("MsSqlConnectionString");
            DbConnection = new SqlConnection(connectionstring.Value);
        }

        public async Task<IEnumerable<TModel>> GetAll()
        {

            return await DbConnection.QueryAsync<TModel>(CreateSeleteString());
        }

        public async Task<TModel> FindById(int id)
        {
            return await DbConnection.QueryFirstAsync<TModel>(CreateSeleteString() + " WHERE Id = @Id", new
            {
                id
            });
        }

        public abstract string CreateSeleteString();
        public abstract Task<int> UpdateAsync(TModel tModel);
        public abstract Task<int> DeleteAsync(int id);
        public abstract Task<int> AddAsync(TModel tModel);
    }

}
