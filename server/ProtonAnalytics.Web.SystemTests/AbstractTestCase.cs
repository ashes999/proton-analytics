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
using Newtonsoft.Json;
using NLog;
using Ingot.Clients;
using System.Text.RegularExpressions;

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

        protected WebClient GetHtmlUnitAuthenticatedClient(string userName)
        {
            var client = GetHtmlUnitClient();
            client.LogIn(userName, this.PasswordFor(userName));
            return client;
        }

        protected User GetAnyUser()
        {
            var firstUser = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            return firstUser;
        }

        protected WebClient GetHtmlUnitClient()
        {
            var client = new WebClient(BrowserVersion.CHROME);
            return client;
        }

        protected HttpClient GetAuthenticatedClient(string userName)
        {
            var client = new HttpClient(string.Format(AbstractTestCase.WebsiteUrl, Environment.MachineName));

            var loginPage = client.Request("GET", "/Account/Login");
            var regex = new Regex(@"<input name=""__RequestVerificationToken"".*value=""([^""]+)""");
            var antiForgeryToken = regex.Match(loginPage.Content()).Groups[1].Value;

            var isAuthed = client.Request("POST", "/Account/Login", new Dictionary<string, string>()
            {
                { "__RequestVerificationToken", antiForgeryToken },
                { "UserName", userName },
                { "Password", "p@ssw0rd!" },
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

    static class WebClientExtensions
    {
        public static NHtmlUnit.Html.HtmlPage GetSiteUrl(this WebClient client, string relativeUrl)
        {
            // substitute in machine name and URL (machine name means we can see requests/responses in Fiddler).
            // eg. http://{0}/ProtonAnalytics.Web/{1} => http://winpc01/ProtonAnalytics.web/Account/Login
            var page = client.GetHtmlPage(string.Format(AbstractTestCase.WebsiteUrl, Environment.MachineName, relativeUrl));
            return page;
        }

        public static NHtmlUnit.IPage GetSiteUrl(this WebClient client, string relativeUrl, string httpVerb, object body)
        {
            var method = ConvertToEnum(httpVerb);
            var json = JsonConvert.SerializeObject(body);

            var url = new java.net.URL(string.Format(AbstractTestCase.WebsiteUrl, Environment.MachineName, relativeUrl));
            var webRequest = new WebRequest(url, new HttpMethod(method))
            {
                RequestBody = "=" + json // WebAPI expects this. Don't ask me why.
            };

            AbstractTestCase.Logger.Info(string.Format("{0} {1}", httpVerb, relativeUrl));
            if (body != null)
            {
                AbstractTestCase.Logger.Info("Body:\n" + json);
            }
            var page = client.GetPage(webRequest);
            return page;
        }

        private static com.gargoylesoftware.htmlunit.HttpMethod ConvertToEnum(string httpVerb)
        {
            switch (httpVerb.ToUpper())
            {
                case "GET":
                    return com.gargoylesoftware.htmlunit.HttpMethod.GET;
                case "POST":
                    return com.gargoylesoftware.htmlunit.HttpMethod.POST;
                case "PUT":
                    return com.gargoylesoftware.htmlunit.HttpMethod.PUT;
                case "DELETE":
                    return com.gargoylesoftware.htmlunit.HttpMethod.DELETE;
                default:
                    throw new ArgumentException(string.Format("Not sure how to convert '{0}' to an HTTP verb", httpVerb));
            }
        }

        public static IPage LogIn(this WebClient client, string userName, string password)
        {
            // substitute in machine name and URL (machine name means we can see requests/responses in Fiddler).
            // eg. http://{0}/ProtonAnalytics.Web/{1} => http://winpc01/ProtonAnalytics.web/Account/Login
            var page = client.GetSiteUrl("/Account/Login");
            page.SetTextField("UserName", userName);
            page.SetTextField("Password", password);
            var toReturn = page.ClickSubmit();
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

        public static IPage ClickSubmit(this HtmlPage page)
        {
            return page.GetHtmlElementById("submit").Click();
        }
    }
}
