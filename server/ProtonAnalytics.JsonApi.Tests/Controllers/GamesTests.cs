using Newtonsoft.Json;
using NUnit.Framework;
using ProtonAnalytics.JsonApi.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtonAnalytics.JsonApi.Tests.Controllers
{
    [TestFixture]
    class GamesControllerTests
    {
        [Test]
        public void IndexShowsAllGames()
        {
            // TODO: how do we do this once we have DI and non-trivial construction?
            var controller = new GamesController();
            var actual = controller.Get();
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Errors, Is.Null);
            Assert.That(actual.Data, Is.Not.Null);

            var allData = actual.Data;
            Assert.That(allData.Count, Is.GreaterThan(0));

            var game = allData[0];
            Assert.That(game.Id, Is.Not.Null);
            Assert.That(game.Name.Length, Is.GreaterThan(1));
            Assert.That(game.OwnerId, Is.GreaterThan(0));
        }
    }
}
