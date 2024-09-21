using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using RailwayApp.Models;
using Rotativa;

namespace RailwayApp.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                //var countRes = db.Reservations.Where(x => x.Booked != true && x.Expiry > DateTime.Now).Count();
                var cust = CustomersController.GetCustomerId(User);
                //var reservations = db.Reservations.Include(x => x.DayToDayTrainRoute).Include(x => x.Customer).Where(x => x.CustomerId == cust).ToList();
                var reservations = db.Reservations.Include(x => x.Route).Include(x => x.Customer).Where(x => x.CustomerId == cust).ToList();
                return View(reservations);
            }
            else if (User.IsInRole("Admin"))
            {
                var reservations = db.Reservations.Include(x => x.Route).Include(x => x.Customer);
                //var reservations = db.Reservations.Include(x => x.DayToDayTrainRoute).Include(x => x.Customer);
                return View(reservations.ToList());
            }
            return View(new List<Reservation>());
        }

        public ActionResult IndexToPdf(string role)
        {
            var reservationsToPrint = new List<Reservation>();
            if (role.Equals("Customer"))
            {
                //var countRes = db.Reservations.Where(x => x.Booked != true && x.Expiry > DateTime.Now).Count();
                var cust = CustomersController.GetCustomerId(User);
                reservationsToPrint = db.Reservations.Include(x => x.Route).Include(x => x.Customer).Where(x => x.CustomerId == cust).ToList();
            }
            else if (role.Equals("Admin"))
            {
                reservationsToPrint = db.Reservations.Include(x => x.Route).Include(x => x.Customer).ToList();
            }
            return View(reservationsToPrint);
        }

        public ActionResult ReservationToPdf(int id)
        {
            if (id == 0)
            {
                if (User.IsInRole("Customer"))
                {
                    var printpdf = new ActionAsPdf("IndexToPdf", new { role = "Customer" });
                    return printpdf;
                }
                else
                {
                    var printpdf = new ActionAsPdf("IndexToPdf", new { role = "Admin" });
                    return printpdf;
                }
            }
            else
            {
                Reservation reservation = db.Reservations.Include(x => x.Route).Include(x => x.Customer).FirstOrDefault(x => x.Id == id);
                if (reservation == null)
                {
                    return HttpNotFound();
                }
                var printpdf = new ActionAsPdf("DetailsToPdf", new { id });
                return printpdf;

            }
        }

        [Authorize]
        public ActionResult Reserve(int dtdId,int tcktl)
        {
            ViewBag.ticketsLeft = tcktl;
            var dayToDayTrainRoutes = db.DayToDayTrainRoutes.Where(a => a.Id == dtdId).Include(d => d.Route).Include(d => d.Train);
            return View(dayToDayTrainRoutes.ToList());
        }
        [HttpPost]
        public ActionResult Reserve(int dtdId, string numTckt)
        {
            var dayToDayTrainRoutes = db.DayToDayTrainRoutes.Where(a => a.Id == dtdId).Include(d => d.Route).Include(d => d.Train);
            try
            {
                Reservation reservation = new Reservation()
                {
                    Booked = false,//tbupdtd to false redirect to payment
                    Expiry = DateTime.Now.AddMinutes(10),
                    IsActive = true,
                    CustomerId = CustomersController.GetCustomerId(User),
                    SingleReferenceNumber = RefGen.ResReferenceGenerator(true),
                    GroupReferenceNumber = RefGen.ResReferenceGenerator(false),
                    DayToDayTrainRouteId = dtdId,
                    NoOfReservations = Convert.ToInt32(numTckt),
                    CreatedDateTime = DateTime.Now,
                    RouteId = dayToDayTrainRoutes.FirstOrDefault().RouteId
                };
                db.Reservations.Add(reservation);
                db.SaveChanges();
                //tbupdtd to redirect to payment
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                var d2dreservations = db.Reservations.Where(a => a.DayToDayTrainRouteId == dtdId).Sum(a => a.NoOfReservations);

                ViewBag.ticketsLeft = dayToDayTrainRoutes.FirstOrDefault().Train.MaxNoOfPassengers - d2dreservations;
                //error occured
                return View(dayToDayTrainRoutes.ToList());
            }
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Include(x => x.Route).Include(x => x.Customer).FirstOrDefault(x => x.Id == id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }
        // GET: Reservations/Details/5
        public ActionResult DetailsToPdf(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Include(x => x.Route).Include(x => x.Customer).FirstOrDefault(x => x.Id == id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            //ViewBag.RouteId = new SelectList(db.Routes.Include(x => x.Train), "Id", "RouteNameDrp");
            //var t = new SelectList(db.Routes.Include(x => x.Train), "Id", "RouteName").ToList();
            ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName");
            //(from r in db.Routes
            //               join t in db.Trains on r.TrainId equals t.Id
            //               select new SelectListItem
            //               {
            //                   Text = r.To + " , " + r.From + " -- " + t.Name,
            //                   Value = r.Id.ToString()
            //               });
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (!User.IsInRole("Customer"))
                {
                    ModelState.AddModelError("", "Only customers can make reservations");
                }
                else
                {
                    reservation.Booked = false;
                    reservation.Expiry = DateTime.Now.AddMinutes(10);
                    reservation.CreatedDateTime = DateTime.Now;
                    reservation.IsActive = true;
                    reservation.CustomerId = CustomersController.GetCustomerId(User);
                    reservation.SingleReferenceNumber = RefGen.ResReferenceGenerator(true);
                    reservation.GroupReferenceNumber = RefGen.ResReferenceGenerator(false);

                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", reservation.CustomerId);
            //ViewBag.RouteId = new SelectList(db.Routes.Include(x => x.Train), "Id", "RouteName" + " " + "Train.Name", reservation.RouteId);
            //ViewBag.RouteId = (from r in db.Routes
            //                   join t in db.Trains on r.TrainId equals t.Id
            //                   select new SelectListItem
            //                   {
            //                       Text = r.To + " , " + r.From + " -- " + t.Name,
            //                       Value = r.Id.ToString(),
            //                       Selected = (r.Id == reservation.RouteId ? true : false),

            //                   });

            ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName");
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", reservation.CustomerId);
            ViewBag.RouteId = new SelectList(db.Routes, "Id", "To", reservation.RouteId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,RouteId,IsActive,CreatedDateTime,ModifiedDateTime")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", reservation.CustomerId);
            ViewBag.RouteId = new SelectList(db.Routes, "Id", "To", reservation.RouteId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public string GetRef(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var refno = context.Payments.FirstOrDefault(x => x.ReservationId == id);
            if(refno == null)
            {
                return "";
            }
            return refno.RefNo;
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
