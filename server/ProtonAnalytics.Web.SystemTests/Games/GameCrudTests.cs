using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var gameName = "FT-FunctionalTestGame";
            this.ExecuteQuery("DELETE FROM Game WHERE Name = @name", new { name = gameName });

            var user = this.ExecuteScalar<User>("SELECT * FROM " + this.UserTableName);
            var client = this.GetAuthenticatedClient(user.UserName);
            var page = client.GetSiteUrl("/Game/Create");
            page.SetTextField("Name", gameName);
            page.ClickSubmit();

            var count = this.ExecuteScalar<int>("SELECT COUNT(*) FROM Game WHERE Name = @name", new { name = gameName });
            Assert.That(count, Is.EqualTo(1), "Didn't see game in DB after creating it");
        }
    }
}
