using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProtonAnalytics.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("http://localhost/ProtonAnalytics.JsonApi/api/values");

                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = content;
                } else {
                    ViewBag.Message = "An error occurred.";
                }
            }

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
