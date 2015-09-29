using ProtonAnalytics.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using ProtonAnalytics.JsonApiClient;

namespace ProtonAnalytics.Web.Controllers
{
    public class GameController : AuthorizedController
    {
        //
        // GET: /Game/
        public ActionResult Index()
        {
            var jsonObject = new JsonHttpClient().Get("/api/games");
            if (jsonObject.Data == null)
            {
                // Do we have errors?
                if (jsonObject.Errors.Length > 0)
                {
                    ViewBag.Flash = string.Join(",", jsonObject.Errors);
                }
                return View(new List<Game>());
            }
            else
            {
                var games = new List<Game>();
                foreach (var json in jsonObject.Data)
                {
                    games.Add(new Game() { Id = json.Id, Name = json.Name, OwnerId = json.OwnerId });
                }
                return View(games);
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
            return View(new Game() { OwnerId = this.CurrentUserId });
        }

        //
        // POST: /Game/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
