using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using ProtonAnalytics.Web.Tests;
using Newtonsoft.Json;
using NLog;
using Ingot.Clients;
using System.Text.RegularExpressions;
using System.Web;

namespace ProtonAnalytics.Web.Tests
{
    abstract class AbstractTestCase
    {
        internal readonly string UserTableName = "UserProfile";
        internal const string WebsiteUrl = "http://{0}/ProtonAnalytics.Web";
        internal static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void ExecuteQuery(string sql, object parameters = null)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
        }

        public T ExecuteScalar<T>(string sql, object parameters = null)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(sql, parameters).FirstOrDefault();
            }
        }
        
        protected string EnsureUserHasGame(User user)
        {
            var gameName = this.ExecuteScalar<string>("SELECT Name FROM Game WHERE OwnerId = @id", new { id = user.UserId });
            if (string.IsNullOrEmpty(gameName))
            {
                gameName = "ST-FirstGame-" + user.UserName;
                this.ExecuteQuery("INSERT INTO Game (Id, Name, OwnerId) VALUES (newid(), @name, @id)", new { id = user.UserId, name = gameName });
            }
            return gameName;
        }

        protected User GetAnyUser()
        {
            var firstUser = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            return firstUser;
        }

        protected HttpClient GetClient()
        {
            var client = new HttpClient(string.Format(AbstractTestCase.WebsiteUrl, Environment.MachineName));
            return client;
        }

        protected HttpClient GetAuthenticatedClient(string userName, string password = "p@ssw0rd!")
        {
            var client = this.GetClient();
            var loginPage = client.Request("GET", "/Account/Login");
            var regex = new Regex(@"<input name=""__RequestVerificationToken"".*value=""([^""]+)""");
            var antiForgeryToken = regex.Match(loginPage.Content()).Groups[1].Value;

            var isAuthed = client.Request("POST", "/Account/Login", new Dictionary<string, string>()
            {
                { "__RequestVerificationToken", antiForgeryToken },
                { "UserName", userName },
                { "Password", password },
                { "RememberMe", "false" }
            });

            return client;
        }

        protected class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
        }
    }

    static class StringExtensions
    {
        public static string HtmlEncode(this string s)
        {
            return System.Web.HttpUtility.HtmlEncode(s);
        }
    }
}
