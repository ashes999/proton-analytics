using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;

namespace ProtonAnalytics.Web.Persistence
{
    public static class DatabaseReader
    {
        public static T ExecuteScalar<T>(string sql, object parameters = null)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                T toReturn = connection.ExecuteScalar<T>(sql, parameters);
                return toReturn;
            }
        }

        public static IEnumerable<T> GetAll<T>(string sql, object parameters = null)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<T>(sql, parameters);
            }
        }
    }
}