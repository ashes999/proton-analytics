using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtonAnalytics.Web.Persistence.Migrations
{
    [Migration(1)]
    public class CreateGameTable_001 : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Game")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString(64)
                .WithColumn("OwnerId").AsInt32().ForeignKey("UserProfile", "UserId");
        }
    }
}