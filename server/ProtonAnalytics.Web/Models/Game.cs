using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProtonAnalytics.Web.Models
{
    public class Game
    {
        public Guid Id { get; private set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]        
        public string Name { get; set; }
        public int OwnerId { get; private set; }

        public Game(int userId)
        {
            this.Id = Guid.NewGuid();
            this.OwnerId = userId;
        }
    }
}