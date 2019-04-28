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
            DbConnection = new SqlConnection("Data Source = GEOGERR; Initial Catalog = PRODUCTM; Integrated Security = true;");
        }

        public IEnumerable<TModel> GetAll()
        {

            return DbConnection.Query<TModel>(CreateSeleteString()).ToList();
        }

        public TModel FindById(int id)
        {
            return this.DbConnection.Query<TModel>(CreateSeleteString() + " WHERE Id = @Id", new
            {
                id
            }).FirstOrDefault();
        }

        public abstract string CreateSeleteString();
        public abstract int Update(TModel tModel);
        public abstract int Delete(int id);
        public abstract int Add(TModel tModel);
    }
    
}
