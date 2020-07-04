using System.Data;
using System.Data.SqlClient;

namespace AtelierEntertainment.Factories
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetSqlConnection(string connectionString);
    }
}