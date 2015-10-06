using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using NHtmlUnit;
using ProtonAnalytics.Web.Tests;
using NHtmlUnit.Html;

namespace ProtonAnalytics.Web.Tests
{
    abstract class AbstractTestCase
    {
        internal readonly string UserTableName = "UserProfile";
        internal const string WebsiteUrl = "http://{0}/ProtonAnalytics.Web/{1}";

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

        // For test environments, we have predictable passwords.
        public string PasswordFor(string userName)
        {
            string password = userName;
            if (userName.Contains('@'))
            {
                password = userName.Substring(0, userName.IndexOf('@'));
            }

            string original = password;
            // If it's too short, keep concatenating itself.
            while (password.Length < 6)
            {
                password = string.Format("{0}{1}", password, original);
            }

            return password;
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

        protected WebClient GetAuthenticatedClient(string userName)
        {
            var client = new WebClient(BrowserVersion.CHROME);
            client.LogIn(userName, this.PasswordFor(userName));
            return client;
        }

        protected class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
        }
    }

    static class WebClientExtensions
    {
        public static NHtmlUnit.Html.HtmlPage GetSiteUrl(this WebClient client, string relativeUrl)
        {
            // substitute in machine name and URL (machine name means we can see requests/responses in Fiddler).
            // eg. http://{0}/ProtonAnalytics.Web/{1} => http://winpc01/ProtonAnalytics.web/Account/Login
            var page = client.GetHtmlPage(string.Format(AbstractTestCase.WebsiteUrl, Environment.MachineName, relativeUrl));
            return page;
        }

        public static IPage LogIn(this WebClient client, string userName, string password)
        {
            // substitute in machine name and URL (machine name means we can see requests/responses in Fiddler).
            // eg. http://{0}/ProtonAnalytics.Web/{1} => http://winpc01/ProtonAnalytics.web/Account/Login
            var page = client.GetSiteUrl("/Account/Login");
            page.SetTextField("UserName", userName);
            page.SetTextField("Password", password);
            var toReturn = page.GetHtmlElementById("submit").Click();
            if (toReturn.IsHtmlPage() && ((HtmlPage)toReturn).AsText().ToUpper().Contains("LOG IN"))
            {
                Console.WriteLine(toReturn.WebResponse.GetContentAsString("ASCII"));
                throw new InvalidOperationException("Login failed as " + userName + " with password " + password);
            }
            return toReturn;
        }

        public static void SetTextField(this HtmlPage page, string id, string value)
        {
            var element = page.GetHtmlElementById(id);
            if (element is HtmlPasswordInput)
            {
                ((HtmlPasswordInput)element).Text = value;
            }
            else if (element is HtmlTextInput)
            {
                ((HtmlTextInput)element).Text = value;
            }
            else
            {
                throw new InvalidOperationException("Not sure how to set text on " + id + ": " + element);
            }
        }
    }
}
