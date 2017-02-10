using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DaliCMS.Models;
using MvcPaging;

namespace DaliCMS.Controllers
{
    public class ResponsibleAdultsController : Controller
    {
        private DaliCMSContext db = new DaliCMSContext();

        // GET: ResponsibleAdults
        public ActionResult Index(int? page)
        {
            int DefaultPageSize = 10;
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(db.ResponsibleAdults.OrderBy(o => o.Name).ToPagedList(currentPageIndex, DefaultPageSize));
            //return View(db.ResponsibleAdults.ToList());
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
