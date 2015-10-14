using ProtonAnalytics.Web.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtonAnalytics.Web.Models
{
    public class GameSession
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Platform { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }

        internal bool IsValid()
        {
            var fieldsAreSpecified = this.GameId != Guid.Empty && !string.IsNullOrEmpty(this.Platform) && this.PlayerId != Guid.Empty && this.StartTimeUtc > DateTime.MinValue;
            var doesGameExist = DatabaseMediator.ExecuteScalar<int>("SELECT COUNT(*) FROM Game WHERE Id = @id", this.GameId) == 1;
            return fieldsAreSpecified && doesGameExist;
        }
    }
}