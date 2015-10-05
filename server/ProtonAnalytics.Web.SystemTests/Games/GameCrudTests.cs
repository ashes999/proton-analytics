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
    }
}
