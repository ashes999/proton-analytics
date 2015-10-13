﻿using ProtonAnalytics.JsonApi.Api;
using ProtonAnalytics.Web.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using ProtonAnalytics.Web.Models;
using ProtonAnalytics.Web.Controllers.Web;
using Newtonsoft.Json;

namespace ProtonAnalytics.Web.Controllers.Api
{
    [Authorize]
    public class GamesController : ApiController
    {
        // GET api/games
        public JsonApiObject<Game> Get()
        {
            var all = DatabaseMediator.GetAll<Game>("SELECT * FROM Game WHERE OwnerId = @me", new { me = this.GetCurrentUserId() });
            return new JsonApiObject<Game>(all);
        }

        // GET api/games/5
        public JsonApiObject<Game> Get(Guid id)
        {
            return null;
        }

        // POST api/games
        public JsonApiObject<Game> Post([FromBody]string json)
        {
            var game = JsonConvert.DeserializeObject<Game>(json);
            game.OwnerId = this.GetCurrentUserId();

            var toReturn = new JsonApiObject<Game>();

            if (game.IsValid())
            {
                toReturn.Data = new List<Game> { game };
                DatabaseMediator.ExecuteQuery("INSERT INTO Game (Id, Name, OwnerId) VALUES (@Id, @Name, @OwnerId)", game);
            }
            else
            {
                toReturn.Errors = new string[] { "Validation failed." };
            }

            return toReturn;
        }

        // PUT api/games/5
        public JsonApiObject<Game> Put(Guid id, [FromBody]string json)
        {
            return null;
        }

        // DELETE api/games/5
        public JsonApiObject<Game> Delete(Guid id)
        {
            return null;
        }
    }
}