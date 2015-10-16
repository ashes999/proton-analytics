using NUnit.Framework;
using ProtonAnalytics.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtonAnalytics.Web.Tests.Games
{
    [TestFixture]
    class GameSessionTests : AbstractTestCase
    {
        [Test]
        public void PostCreatesNewGameSessionsWhenFieldsAreValid()
        {
            var client = this.GetHtmlUnitClient();
            var user = this.GetAnyUser();
            var game = this.EnsureUserHasGame(user);

            var newSession = new GameSession() {
                Id = Guid.NewGuid(),
                
            };

            //client.GetSiteUrl("/GameSession", "POST", newSession)
        }

        [Test]
        public void PutSetsEndSessionTime()
        {

        }
    }
}
