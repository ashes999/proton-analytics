using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace ProtonAnalytics.Web.Controllers
{
    [Authorize]
    public abstract class AuthenticatedController : Controller
    {
        // TODO: move to API
        private int currentUserId = 0;
        public int CurrentUserId
        {
            get
            {
                if (currentUserId == 0)
                {
                    var userName = User.Identity.Name;
                    currentUserId = 999;
                }

                return currentUserId;
            }
        }
	}
}