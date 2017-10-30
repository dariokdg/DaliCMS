using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DaliCMS.Models;
using X.PagedList;

namespace DaliCMS.Controllers
{
    public class PaymentsController : Controller
    {
        private DaliCMSContext db = new DaliCMSContext();

        // GET: Payments
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var payments = db.Payments.Include(p => p.StudentModel);

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
                payments = payments.Where(s => s.StudentModel.Name.Contains(searchString));
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "Name";
            ViewBag.AmntSortParm = sortOrder == "Amount" ? "Amount_desc" : "Amount";
            ViewBag.MnthSortParm = sortOrder == "MonthYearPaid" ? "" : "MonthYearPaid";
            ViewBag.DsctSortParm = sortOrder == "HasDiscount" ? "HasDiscount_desc" : "HasDiscount";
            ViewBag.PnltSortParm = sortOrder == "HasPenalty" ? "HasPenalty_desc" : "HasPenalty";
            ViewBag.DateSortParm = sortOrder == "PaymentDate" ? "PaymentDate_desc" : "PaymentDate";

            switch (sortOrder)
            {
                case "Name":
                    payments = payments.OrderBy(s => s.StudentModel.Name);
                    break;
                case "Name_desc":
                    payments = payments.OrderByDescending(s => s.StudentModel.Name);
                    break;
                case "Amount":
                    payments = payments.OrderBy(s => s.Amount);
                    break;
                case "Amount_desc":
                    payments = payments.OrderByDescending(s => s.Amount);
                    break;
                case "MonthYearPaid":
                    payments = payments.OrderBy(s => s.MonthYearPaid);
                    break;
                case "HasDiscount":
                    payments = payments.OrderBy(s => s.HasDiscount);
                    break;
                case "HasDiscount_desc":
                    payments = payments.OrderByDescending(s => s.HasDiscount);
                    break;
                case "HasPenalty":
                    payments = payments.OrderBy(s => s.HasPenalty);
                    break;
                case "HasPenalty_desc":
                    payments = payments.OrderByDescending(s => s.HasPenalty);
                    break;
                case "PaymentDate":
                    payments = payments.OrderBy(s => s.PaymentDate);
                    break;
                case "PaymentDate_desc":
                    payments = payments.OrderByDescending(s => s.PaymentDate);
                    break;
                default:
                    payments = payments.OrderByDescending(s => s.MonthYearPaid);
                    break;
            }

            int DefaultPageSize = 10;
            int currentPageIndex = (page ?? 1);
            return View(payments.ToPagedList(currentPageIndex, DefaultPageSize));
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
        public ActionResult Create([Bind(Include = "Id,Amount,CancellationAmount,HasDiscount,HasPenalty,PaymentDate,MonthYearPaid,StudentId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                Student student = db.Students.Find(payment.StudentId);
                if (student == null)
                {
                    return HttpNotFound();
                }
                decimal adjustment = 0;
                payment.HasDiscount = false;
                payment.HasPenalty = false;
                if (payment.PaymentDate.Day <= 10 && (payment.PaymentDate.Month == payment.MonthYearPaid.Month && payment.PaymentDate.Year == payment.MonthYearPaid.Year))
                {
                    adjustment = adjustment + 5;
                    payment.HasDiscount = true;
                }
                else if (payment.PaymentDate.Month < payment.MonthYearPaid.Month || payment.PaymentDate.Year < payment.MonthYearPaid.Year)
                {
                    adjustment = adjustment + 5;
                    payment.HasDiscount = true;
                }
                else if (payment.PaymentDate.Month > payment.MonthYearPaid.Month || payment.PaymentDate.Year > payment.MonthYearPaid.Year)
                {
                    adjustment = adjustment - 10;
                    payment.HasPenalty = true;
                }
                List<StudentRespAdultRel> ResponsibleAdultsRel = student.StudentRespAdultRels.Where(Rel => Rel.StudentId == student.Id).ToList();
                foreach (StudentRespAdultRel RespAdultRel in ResponsibleAdultsRel)
                {
                    ResponsibleAdult RA = RespAdultRel.ResponsibleAdultModel;
                    if (RA.StudentRespAdultRels.Where(Rel => Rel.ResponsibleAdultId == RA.Id).ToList().Count() > 1)
                    {
                        adjustment = adjustment + 20;
                        payment.HasDiscount = true;
                        break;
                    }
                }
                payment.CancellationAmount = payment.Amount / (100 - adjustment) * 100;
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

        [HttpPost]
        public JsonResult GetStudentDebt(string StudentId)
        {
            int Student = Convert.ToInt32(StudentId);
            Student SelectedStudent = db.Students.FirstOrDefault(s => s.Id == Student);
            if (SelectedStudent != null)
            {
                return Json(new { success = true, studentDebt = Convert.ToDecimal(SelectedStudent.Debt) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false });
        }

        //This implementation of the "GetStudentSiblings" is working but it is painfully slow - need to make it faster.
        [HttpPost]
        public JsonResult GetStudentSiblings(string StudentId)
        {
            int Student = Convert.ToInt32(StudentId);
            Student SelectedStudent = db.Students.FirstOrDefault(s => s.Id == Student);
            if (SelectedStudent != null)
            {
                StudentRespAdultRel relations = SelectedStudent.StudentRespAdultRels.FirstOrDefault();
                int SignedUpChildrenPerRespAdult = 0;
                if (relations != null)
                {
                    SignedUpChildrenPerRespAdult = relations.ResponsibleAdultModel.StudentRespAdultRels.Count();
                }
                return Json(new { success = true, studentSiblings = SignedUpChildrenPerRespAdult >= 2 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false });
        }
    }
}
