using System.Data;
using System.Data.SqlClient;

namespace AtelierEntertainment.Factories
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        public IDbConnection GetSqlConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
