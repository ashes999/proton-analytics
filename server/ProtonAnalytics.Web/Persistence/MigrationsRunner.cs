using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ProtonAnalytics.Web.Persistence
{
    public static class MigrationsRunner
    {
        public static void MigrateToLatest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var migrationAssembly=  Assembly.GetExecutingAssembly();
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var migrationContext = new RunnerContext(announcer);
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();
            var options = new MigrationOptions { PreviewOnly = false, Timeout = 0 };
            var processor = factory.Create(connectionString, announcer, options);

            var runner = new MigrationRunner(migrationAssembly, migrationContext, processor);
            runner.MigrateUp();
        }

        private class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public int Timeout { get; set; }
            public string ProviderSwitches { get; set; }
        }
    }
}