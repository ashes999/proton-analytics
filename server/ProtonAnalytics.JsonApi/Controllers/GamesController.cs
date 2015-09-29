using ProtonAnalytics.JsonApi.Persistence;
using ProtonAnalytics.JsonApi.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProtonAnalytics.JsonApi.Controllers
{
    public class GamesController : ApiController
    {
        // GET api/games
        public JsonApiObject Get()
        {
            var all = DatabaseReader.GetAll<dynamic>("SELECT * FROM Game");
            return new JsonApiObject(all);
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