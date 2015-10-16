using Ingot.Clients;
using Newtonsoft.Json;
using NUnit.Framework;
using ProtonAnalytics.Web.Api;
using ProtonAnalytics.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ProtonAnalytics.Web.Tests.Games
{
    [TestFixture]
    class GameCrudTests : AbstractTestCase
    {
        #region forms tests

        [Test]
        public void GameIndexPageShowsOnlyMyGames()
        {
            // Pick any two users. Make sure both have games.  Log in as one. This fails if there's only one user.
            var firstUser = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            var firstUsersGame = this.EnsureUserHasGame(firstUser).HtmlEncode();
            
            var secondUser = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName + " WHERE UserId != @first", new { first = firstUser.UserId });
            var secondUsersGame = this.EnsureUserHasGame(secondUser).HtmlEncode();

            var client = this.GetAuthenticatedClient(firstUser.UserName);

            var html = client.Request("GET", "/Game").Content();
            Assert.IsTrue(html.Contains(firstUsersGame), "Expected to see " + firstUsersGame + " in the HTML but didn't.\n" + html);
            Assert.IsFalse(html.Contains(secondUsersGame));
        }

        [Test]
        public void GameCreatePageCreatesGameInDb()
        {
            var gameName = "FT-FunctionalTestGameView";
            this.ExecuteQuery("DELETE FROM Game WHERE Name = @name", new { name = gameName });

            var user = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            var client = this.GetAuthenticatedClient(user.UserName);
            var page = client.Request("POST", "/Game/Create", new Dictionary<string, string>()
            {
                { "Name", gameName },
                { "OwnerId", user.UserId.ToString() }
            });

            var count = this.ExecuteScalar<int>("SELECT COUNT(*) FROM Game WHERE Name = @name", new { name = gameName });
            Assert.That(count, Is.EqualTo(1), "Didn't see game in DB after creating it");
        }
        
        #endregion

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
            var gameJson = "=" + JsonConvert.SerializeObject(game);


            var page = client.Request("POST", "/api/games", gameJson);

            var count = this.ExecuteScalar<int>("SELECT COUNT(*) FROM Game WHERE Name = @name", new { name = gameName });
            Assert.That(count, Is.EqualTo(1), "Didn't see game in DB after creating it");
        }

        [Test]
        public void FormsAuthenticatedUserCanGetAllGamesAndPostGames()
        {
            var user = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            var game = this.EnsureUserHasGame(user);
            var client = this.GetAuthenticatedClient(user.UserName);
            
            string url = "/api/games";                        
            var result = client.Request("GET", url);
            Assert.IsNotNull(result);
            var listOfGames = JsonConvert.DeserializeObject<JsonApiObject<Game>>(result.Content());

            Assert.That(listOfGames.Data, Is.Not.Null);
        }
    }
}
