using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProtonAnalytics.JsonApi.Models;

namespace ProtonAnalytics.JsonApi.Controllers
{
    public class UserController : ApiController
    {
        public User LogIn(string userName, string plainTextPassword)
        {
            // TODO: authenticate.
            return new User(99999, userName);
        }
    }
}
