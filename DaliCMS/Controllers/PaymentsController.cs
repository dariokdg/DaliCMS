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
    public class PaymentsController : Controller
    {
        private DaliCMSContext db = new DaliCMSContext();

        // GET: Payments
        public ActionResult Index(int? page)
        {
            int DefaultPageSize = 10;
            var payments = db.Payments.Include(p => p.StudentModel);
            int currentPageIndex = (page ?? 1);
            return View(payments.OrderBy(o => o.PaymentDate).ToPagedList(currentPageIndex, DefaultPageSize));
            //var payments = db.Payments.Include(p => p.StudentModel);
            //return View(payments.ToList());
        }

        // GET: Payments/Create
        public ActionResult Create(int StudentId = 0)
        {
            if (StudentId == 0)
            {
                ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
            }
            else
            {
                ViewBag.StudentId = new SelectList(db.Students, "ID", "Name", db.Students.Select(x => x.Id == StudentId).FirstOrDefault());
            }
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Amount,CancellationAmount,HasDiscount,PaymentDate,StudentId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                Student student = db.Students.Find(payment.StudentId);
                if (student == null)
                {
                    return HttpNotFound();
                }
                decimal discount = 0;
                payment.HasDiscount = false;
                if (payment.PaymentDate.Day <= 10 && payment.PaymentDate.Month == DateTime.Now.Month)
                {
                    discount = 10;
                    payment.HasDiscount = true;
                }
                payment.CancellationAmount = payment.Amount / (100 - discount) * 100;
                student.Debt = student.Debt - payment.CancellationAmount;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", payment.StudentId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            Student student = db.Students.Find(payment.StudentId);
            if (student == null)
            {
                return HttpNotFound();
            }
            student.Debt = student.Debt + payment.CancellationAmount;
            db.Payments.Remove(payment);
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
