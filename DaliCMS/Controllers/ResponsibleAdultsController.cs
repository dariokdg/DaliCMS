using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DaliCMS.Models;
using X.PagedList;

namespace DaliCMS.Controllers
{
    public class ResponsibleAdultsController : Controller
    {
        private DaliCMSContext db = new DaliCMSContext();

        // GET: ResponsibleAdults
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            var responsibleadults = (IQueryable<ResponsibleAdult>)db.ResponsibleAdults;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                responsibleadults = responsibleadults.Where(s => s.Name.Contains(searchString));
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.AddrSortParm = sortOrder == "Address" ? "Address_desc" : "Address";
            ViewBag.PhneSortParm = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewBag.JobNSortParm = sortOrder == "Job" ? "Job_desc" : "Job";

            switch (sortOrder)
            {
                case "Name_desc":
                    responsibleadults = responsibleadults.OrderByDescending(s => s.Name);
                    break;
                case "Address":
                    responsibleadults = responsibleadults.OrderBy(s => s.Address);
                    break;
                case "Address_desc":
                    responsibleadults = responsibleadults.OrderByDescending(s => s.Address);
                    break;
                case "Phone":
                    responsibleadults = responsibleadults.OrderBy(s => s.Phone);
                    break;
                case "Phone_desc":
                    responsibleadults = responsibleadults.OrderByDescending(s => s.Phone);
                    break;
                case "Job":
                    responsibleadults = responsibleadults.OrderBy(s => s.JobName);
                    break;
                case "Job_desc":
                    responsibleadults = responsibleadults.OrderByDescending(s => s.JobName);
                    break;
                default:
                    responsibleadults = responsibleadults.OrderBy(s => s.Name);
                    break;
            }

            int DefaultPageSize = 10;
            int currentPageIndex = (page ?? 1);
            return View(responsibleadults.ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: ResponsibleAdults/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsibleAdult responsibleAdult = db.ResponsibleAdults.Find(id);
            if (responsibleAdult == null)
            {
                return HttpNotFound();
            }
            return View(responsibleAdult);
        }

        // GET: ResponsibleAdults/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResponsibleAdults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Phone,JobName")] ResponsibleAdult responsibleAdult)
        {
            if (ModelState.IsValid)
            {
                db.ResponsibleAdults.Add(responsibleAdult);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(responsibleAdult);
        }

        // GET: ResponsibleAdults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsibleAdult responsibleAdult = db.ResponsibleAdults.Find(id);
            if (responsibleAdult == null)
            {
                return HttpNotFound();
            }
            return View(responsibleAdult);
        }

        // POST: ResponsibleAdults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,JobName")] ResponsibleAdult responsibleAdult)
        {
            if (ModelState.IsValid)
            {
                db.Entry(responsibleAdult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responsibleAdult);
        }

        // GET: ResponsibleAdults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsibleAdult responsibleAdult = db.ResponsibleAdults.Find(id);
            if (responsibleAdult == null)
            {
                return HttpNotFound();
            }
            return View(responsibleAdult);
        }

        // POST: ResponsibleAdults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResponsibleAdult responsibleAdult = db.ResponsibleAdults.Find(id);
            db.ResponsibleAdults.Remove(responsibleAdult);
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
