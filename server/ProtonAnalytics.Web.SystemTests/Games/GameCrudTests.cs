using Newtonsoft.Json;
using NUnit.Framework;
using ProtonAnalytics.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProtonAnalytics.Web.Tests.Games
{
    [TestFixture]
    class GameCrudTests : AbstractTestCase
    {
        [Test]
        public void GameIndexPageShowsOnlyMyGames()
        {
            // Pick any two users. Make sure both have games.  Log in as one. This fails if there's only one user.
            var firstUser = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            var firstUsersGame = this.EnsureUserHasGame(firstUser);
            
            var secondUser = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName + " WHERE UserId != @first", new { first = firstUser.UserId });
            var secondUsersGame = this.EnsureUserHasGame(secondUser);

            var client = this.GetAuthenticatedClient(firstUser.UserName);

            var page = client.GetSiteUrl("/Game").Body.TextContent;
            Assert.IsTrue(page.Contains(firstUsersGame));
            Assert.IsFalse(page.Contains(secondUsersGame));
        }

        [Test]
        public void GameCreatePageCreatesGameInDb()
        {
            var gameName = "FT-FunctionalTestGameView";
            this.ExecuteQuery("DELETE FROM Game WHERE Name = @name", new { name = gameName });

            var user = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            var client = this.GetAuthenticatedClient(user.UserName);
            var page = client.GetSiteUrl("/Game/Create");
            page.SetTextField("Name", gameName);
            page.ClickSubmit();

            var count = this.ExecuteScalar<int>("SELECT COUNT(*) FROM Game WHERE Name = @name", new { name = gameName });
            Assert.That(count, Is.EqualTo(1), "Didn't see game in DB after creating it");
        }

        [Test]
        public void UserCanPostGame()
        {
            var gameName = "FT-FunctionalTestGame";
            this.ExecuteQuery("DELETE FROM Game WHERE Name = @name", new { name = gameName });

            var user = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            var client = this.GetAuthenticatedClient(user.UserName);
            var game = new Game()
            {
                Name = gameName,
                OwnerId = user.UserId
            };

            var page = client.GetSiteUrl("/api/games", "POST", game);

            var count = this.ExecuteScalar<int>("SELECT COUNT(*) FROM Game WHERE Name = @name", new { name = gameName });
            Assert.That(count, Is.EqualTo(1), "Didn't see game in DB after creating it");
        }

        [Test]
        public void HtmlUnitDoesntThrowWhenPostsAndGetsAJsonResponse()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://aalibhai-d02/ProtonAnalytics.Web/api/games");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "={\"Name\":\"FT-FunctionalTestGame\",\"OwnerId\":3}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}
