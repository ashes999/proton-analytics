using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;

namespace ProtonAnalytics.JsonApi.Persistence
{
    static class DatabaseReader
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

        public static T GetOne<T>(string sql, object parameters)
        {
            return GetAll<T>(sql, parameters).Single();
        }

        public static IEnumerable<T> GetAll<T>(string sql, object parameters = null)
        {
            IEnumerable<T> toReturn = new List<T>();
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                toReturn = connection.Query<T>(sql, parameters);
            }

            return toReturn;
        }
    }
}