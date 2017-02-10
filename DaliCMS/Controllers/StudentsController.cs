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
    public class StudentsController : Controller
    {
        private DaliCMSContext db = new DaliCMSContext();

        // GET: Students
        public ActionResult Index(int? page)
        {
            int DefaultPageSize = 10;
            var students = db.Students.Include(s => s.LevelModel).Include(s => s.SchoolModel);
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(students.OrderBy(o => o.Name).ToPagedList(currentPageIndex, DefaultPageSize));
            //var students = db.Students.Include(s => s.LevelModel).Include(s => s.SchoolModel);
            //return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "Name");
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DNI,Address,Phone,Dateofbirth,Debt,LevelId,SchoolId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                //CHECK ALL FIELDS
                return RedirectToAction("Index");
            }

            ViewBag.LevelId = new SelectList(db.Levels, "Id", "Name", student.LevelId);
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name", student.SchoolId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "Name", student.LevelId);
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name", student.SchoolId);
            student.Debt = Convert.ToInt32(student.Debt);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DNI,Address,Phone,Dateofbirth,Debt,LevelId,SchoolId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "Name", student.LevelId);
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name", student.SchoolId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
