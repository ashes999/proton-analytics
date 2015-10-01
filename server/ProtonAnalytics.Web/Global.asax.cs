using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProtonAnalytics.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Try to run migrations. This fails if the EF auth tables don't
            // exist yet (first run of the app). That's okay; migrations run
            // after those tables are initialized, elsewhere. We still need
            // this to auto-run migrations at startup time.
            try
            {
                ProtonAnalytics.Web.Persistence.MigrationsRunner.MigrateToLatest();
            }
            catch
            {
                // Gotta catch 'em all ...
            }
        }
    }
}