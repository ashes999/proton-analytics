using ProtonAnalytics.Web.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProtonAnalytics.Web.Controllers
{
    [Authorize]
    public abstract class AuthorizedController : Controller
    {
        protected Dictionary<string, int> userNameToId = new Dictionary<string, int>();

        protected int CurrentUserId
        {
            get
            {
                var userName = User.Identity.Name;
                if (!userNameToId.ContainsKey(userName))
                {
                    var userId = DatabaseReader.ExecuteScalar<int>("SELECT UserId FROM UserProfile WHERE UserName = @name", new { name = userName });
                    userNameToId[userName] = userId;
                }

                return userNameToId[userName];
            }
        }
	}
}