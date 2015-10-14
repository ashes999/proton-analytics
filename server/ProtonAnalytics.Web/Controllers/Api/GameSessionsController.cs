using Newtonsoft.Json;
using ProtonAnalytics.Web.Api;
using ProtonAnalytics.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProtonAnalytics.Web.Controllers.Web;
using ProtonAnalytics.Web.Persistence;

namespace ProtonAnalytics.Web.Controllers.Api
{
    public class GameSessionsController : ApiController
    {
        // POST api/<controller>
        public JsonApiObject<GameSession> Post([FromBody]string json)
        {
            var session = JsonConvert.DeserializeObject<GameSession>(json);

            var toReturn = new JsonApiObject<GameSession>();

            if (session.IsValid())
            {
                toReturn.Data = new List<GameSession> { session };
                DatabaseMediator.ExecuteQuery("INSERT INTO Game (Id, GameId, Platform, PlayerId, StartTimeUtc) VALUES (@Id, @GameId, @Platform, @PlayerId, @StartTimeUtc)", session);
            }
            else
            {
                toReturn.Errors = new string[] { "Validation failed." };
            }

            return toReturn;
        }

        // PUT api/<controller>/5
        public JsonApiObject<GameSession> Put(int id, [FromBody]string value)
        {
            return null;
        }
    }
}