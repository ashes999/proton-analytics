using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProtonAnalytics.Web.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        protected void NullCheck(string json)
        {
            if (json == null)
            {
                throw new ArgumentException("Couldn't deserialize JSON. Make sure you passed it into the request body prepended with '='");
            }
        }
    }
}
