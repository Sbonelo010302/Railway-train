using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RailwayApp.Models;

namespace RailwayApp.Controllers
{
    public class RoutesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Routes
        public ActionResult Index()
        {
            return View(db.Routes.ToList());
        }
        [HttpPost]
        public ActionResult Index(string from, string to)
        {
            if(string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                if(string.IsNullOrEmpty(from))
                {
                    return View(db.Routes.Where(a => a.To.ToLower().Contains( to.ToLower())).ToList());
                }
                else
                {
                    return View(db.Routes.Where(a => a.From.ToLower().Contains(from.ToLower())).ToList());
                }
            }
            else
            {
                 return View(db.Routes.Where(a=>a.From.ToLower().Contains(from.ToLower()) && a.To.ToLower().Contains(to.ToLower())).ToList());
            }
        }

        // GET: Routes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // GET: Routes/Create
        public ActionResult Create()
        {
            ViewBag.Trains = new SelectList(db.Trains, "Id", "Name");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Route route)
        {
            try
            {
                //route.TrainId = int.Parse(Request.Form["Trains"].ToString());
                route.IsActive = true;
                route.CreatedDateTime = DateTime.Now;
                db.Routes.Add(route);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //ViewBag.Trains = new SelectList(db.Trains, "Id", "Name",route.TrainId);
                return View(route);
            }
        }

        // GET: Routes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,To,From,Rate,Sequence,IsActive,Hours,Minutes,CreatedDateTime,ModifiedDateTime")] Route route)
        {
            if (ModelState.IsValid)
            {
                db.Entry(route).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(route);
        }

        // GET: Routes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Route route = db.Routes.Find(id);
            db.Routes.Remove(route);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
