using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Dapper;

namespace ProtonAnalytics.Web.Controllers
{
    [Authorize]
    public abstract class AuthenticatedController : Controller
    {
        // TODO: move to API
        private int currentUserId = 0;
        public int CurrentUserId
        {
            get
            {
                if (currentUserId == 0)
                {
                    var userName = User.Identity.Name;
                    var userId = ExecuteScalar<int>("SELECT UserId FROM UserProfile WHERE UserName = @userName", new { userName = userName });
                    this.currentUserId = userId;
                }

                return currentUserId;
            }
        }

        public T ExecuteScalar<T>(string sql, object param)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<T>(sql, param);
            }
        }
	}
}