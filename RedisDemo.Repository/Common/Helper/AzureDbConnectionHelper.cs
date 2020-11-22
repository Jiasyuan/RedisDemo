using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RedisDemo.Repository.Common.Interface;

namespace RedisDemo.Repository.Common.Helper
{
    public class AzureDbConnectionHelper : IDatabaseConnectionHelper
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public AzureDbConnectionHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            this._connectionString = _configuration.GetSection("ConnectionSetting")
                .Value; ;
        }

        /// <summary>
        /// Create DbConnection
        /// </summary>
        /// <returns></returns>
        public IDbConnection Create()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            return sqlConnection;
        }
    }
}
