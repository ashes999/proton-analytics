using ProtonAnalytics.JsonApi.Api;
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

namespace ProtonAnalytics.Web.Controllers.Api
{
    public class GamesController : ApiController
    {
        // GET api/games
        public JsonApiObject<Game> Get()
        {
            var all = DatabaseReader.GetAll<Game>("SELECT * FROM Game WHERE OwnerId = @me", new { me = this.GetCurrentUserId() });
            return new JsonApiObject<Game>(all);
        }

        // GET api/games/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/games
        public void Post([FromBody]string value)
        {
        }

        // PUT api/games/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/games/5
        public void Delete(int id)
        {
        }
    }
}