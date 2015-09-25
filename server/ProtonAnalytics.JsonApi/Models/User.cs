using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtonAnalytics.JsonApi.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string UserName { get; set; }

        public User(int id, string userName)
        {
            this.Id = id;
            this.UserName = userName;
        }
    }
}