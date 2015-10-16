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
            var firstUsersGame = this.EnsureUserHasGame(firstUser);
            
            var secondUser = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName + " WHERE UserId != @first", new { first = firstUser.UserId });
            var secondUsersGame = this.EnsureUserHasGame(secondUser);

            var client = this.GetHtmlUnitAuthenticatedClient(firstUser.UserName);

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
            var client = this.GetHtmlUnitAuthenticatedClient(user.UserName);
            var page = client.GetSiteUrl("/Game/Create");
            page.SetTextField("Name", gameName);
            page.ClickSubmit();

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
            var client = this.GetHtmlUnitAuthenticatedClient(user.UserName);
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
