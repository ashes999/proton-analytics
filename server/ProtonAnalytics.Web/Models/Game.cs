using ProtonAnalytics.Web.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProtonAnalytics.Web.Models
{
    public class Game
    {
        public Guid Id { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]        
        public string Name { get; set; }

        public int OwnerId { get; set; }

        internal bool IsValid()
        {
            var fieldsAreSpecified = !string.IsNullOrEmpty(this.Name) && this.Name.Length >= 6 && this.OwnerId > 0;
            var doesOwnerExist = DatabaseMediator.ExecuteScalar<int>("SELECT COUNT(*) FROM UserProfile WHERE UserId = @id", new { id = this.OwnerId }) == 1;
            return fieldsAreSpecified && doesOwnerExist;
        }
    }
}