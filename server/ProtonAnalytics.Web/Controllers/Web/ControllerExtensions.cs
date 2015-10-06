using ProtonAnalytics.Web.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ProtonAnalytics.Web.Controllers.Web
{
    public static class ControllerExtensions
    {
        private static Dictionary<string, int> userNameToId = new Dictionary<string, int>();

        public static int GetCurrentUserId(this Controller c)
        {
            var userName = c.User.Identity.Name;
            if (!userNameToId.ContainsKey(userName))
            {
                var userId = DatabaseMediator.ExecuteScalar<int>("SELECT UserId FROM UserProfile WHERE UserName = @name", new { name = userName });
                userNameToId[userName] = userId;
            }

            return userNameToId[userName];
        }

        public static int GetCurrentUserId(this ApiController c)
        {
            var userName = c.User.Identity.Name;
            if (!userNameToId.ContainsKey(userName))
            {
                var userId = DatabaseMediator.ExecuteScalar<int>("SELECT UserId FROM UserProfile WHERE UserName = @name", new { name = userName });
                userNameToId[userName] = userId;
            }

            return userNameToId[userName];
        }
    }
}