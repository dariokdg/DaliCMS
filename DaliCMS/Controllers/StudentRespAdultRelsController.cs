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
    public class StudentRespAdultRelsController : Controller
    {
        private DaliCMSContext db = new DaliCMSContext();

        // GET: StudentRespAdultRels
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var studentsRespAdultsRel = db.StudentsRespAdultsRel.Include(s => s.ResponsibleAdultModel).Include(s => s.StudentModel);

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
                studentsRespAdultsRel = studentsRespAdultsRel.Where(s => s.ResponsibleAdultModel.Name.Contains(searchString) || s.StudentModel.Name.Contains(searchString));
            }

            ViewBag.RsAdSortParm = String.IsNullOrEmpty(sortOrder) ? "ResponsibleAdultName_desc" : "";
            ViewBag.StdtSortParm = sortOrder == "StudentName" ? "StudentName_desc" : "StudentName";

            switch (sortOrder)
            {
                case "ResponsibleAdultName_desc":
                    studentsRespAdultsRel = studentsRespAdultsRel.OrderByDescending(s => s.ResponsibleAdultModel.Name);
                    break;
                case "StudentName":
                    studentsRespAdultsRel = studentsRespAdultsRel.OrderBy(s => s.StudentModel.Name);
                    break;
                case "StudentName_desc":
                    studentsRespAdultsRel = studentsRespAdultsRel.OrderByDescending(s => s.StudentModel.Name);
                    break;
                default:
                    studentsRespAdultsRel = studentsRespAdultsRel.OrderBy(s => s.ResponsibleAdultModel.Name);
                    break;
            }

            int DefaultPageSize = 10;
            int currentPageIndex = (page ?? 1);
            return View(studentsRespAdultsRel.ToPagedList(currentPageIndex, DefaultPageSize));
        }

        // GET: StudentRespAdultRels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentRespAdultRel studentRespAdultRel = db.StudentsRespAdultsRel.Find(id);
            if (studentRespAdultRel == null)
            {
                return HttpNotFound();
            }
            return View(studentRespAdultRel);
        }

        // GET: StudentRespAdultRels/Create
        public ActionResult Create()
        {
            ViewBag.ResponsibleAdultId = new SelectList(db.ResponsibleAdults, "Id", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
            return View();
        }

        // POST: StudentRespAdultRels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,ResponsibleAdultId")] StudentRespAdultRel studentRespAdultRel)
        {
            if (ModelState.IsValid)
            {
                db.StudentsRespAdultsRel.Add(studentRespAdultRel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ResponsibleAdultId = new SelectList(db.ResponsibleAdults, "Id", "Name", studentRespAdultRel.ResponsibleAdultId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", studentRespAdultRel.StudentId);
            return View(studentRespAdultRel);
        }

        // GET: StudentRespAdultRels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentRespAdultRel studentRespAdultRel = db.StudentsRespAdultsRel.Find(id);
            if (studentRespAdultRel == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResponsibleAdultId = new SelectList(db.ResponsibleAdults, "Id", "Name", studentRespAdultRel.ResponsibleAdultId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", studentRespAdultRel.StudentId);
            return View(studentRespAdultRel);
        }

        // POST: StudentRespAdultRels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,ResponsibleAdultId")] StudentRespAdultRel studentRespAdultRel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentRespAdultRel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResponsibleAdultId = new SelectList(db.ResponsibleAdults, "Id", "Name", studentRespAdultRel.ResponsibleAdultId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", studentRespAdultRel.StudentId);
            return View(studentRespAdultRel);
        }

        // GET: StudentRespAdultRels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentRespAdultRel studentRespAdultRel = db.StudentsRespAdultsRel.Find(id);
            if (studentRespAdultRel == null)
            {
                return HttpNotFound();
            }
            return View(studentRespAdultRel);
        }

        // POST: StudentRespAdultRels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentRespAdultRel studentRespAdultRel = db.StudentsRespAdultsRel.Find(id);
            db.StudentsRespAdultsRel.Remove(studentRespAdultRel);
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
