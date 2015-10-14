using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ProtonAnalytics.Web.Persistence.Migrations
{
    [Migration(2)]
    public class CreateGameSessionTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GameSession")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("GameId").AsGuid()
                    .ForeignKey("Game", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("Platform").AsString(32)
                .WithColumn("PlayerId").AsGuid()
                .WithColumn("StartTimeUtc").AsDateTime()
                .WithColumn("EndTimeUtc").AsDateTime().Nullable();
        }
    }
}