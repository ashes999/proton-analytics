using ProtonAnalytics.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using ProtonAnalytics.Web.Controllers.Api;
using Newtonsoft.Json;

namespace ProtonAnalytics.Web.Controllers.Web
{
    [Authorize]
    public class GameController : Controller
    {
        private GamesController apiController = new GamesController();

        //
        // GET: /Game/
        public ActionResult Index()
        {
            var jsonObject = apiController.Get();
            if (jsonObject.Data == null)
            {
                // Do we have errors?
                if (jsonObject.Errors.Any())
                {
                    ViewBag.Flash = string.Join(",", jsonObject.Errors);
                }
                return View(new List<Game>());
            }
            else
            {
                return View(jsonObject.Data);
            }
        }

        //
        // GET: /Game/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Game/Create
        public ActionResult Create()
        {
            return View(new Game() { OwnerId = this.GetCurrentUserId() });
        }

        //
        // POST: /Game/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var json = JsonConvert.SerializeObject(new Game()
            {
                Id = Guid.NewGuid(),
                Name = collection["Name"],
                OwnerId = int.Parse(collection["OwnerId"])
            });

            var jsonObject = apiController.Post(json);
            if (jsonObject.Data == null)
            {
                // Do we have errors?
                if (jsonObject.Errors.Any())
                {
                    ViewBag.Flash = string.Join(",", jsonObject.Errors);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Game/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Game/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Game/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Game/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
