using ProtonAnalytics.JsonApiClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProtonAnalytics.Web.Controllers
{
    public class HomeController : Controller
    {
        JsonHttpClient client = new JsonHttpClient(ConfigurationManager.AppSettings["ApiBaseUrl"]);

        public async Task<ActionResult> Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";            
            var json = await client.Get<DateTime>("/api/values");
            ViewBag.Message = "The time is: " + json.Data.ToString();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
