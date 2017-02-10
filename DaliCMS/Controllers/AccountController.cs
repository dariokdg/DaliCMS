using DaliCMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DaliCMS.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                using (DaliCMSContext db = new DaliCMSContext())
                {
                    var acCheck = (from a in db.Accounts where a.Username == account.Username select a).FirstOrDefault();
                    if (acCheck != null)
                    {
                        ModelState.AddModelError("Register", "> El nombre de usuario ya existe en la base de datos");
                    }
                    else
                    {
                        if (account.Password != account.PasswordCheck)
                        {
                            ModelState.AddModelError("Register", "> Las contraseñas deben coincidir");
                        }
                        else
                        {
                            account.Password = Crypto.HashPassword(account.Password);
                            account.PasswordCheck = account.Password;
                            db.Accounts.Add(account);
                            db.SaveChanges();
                            ModelState.Clear();
                            ViewBag.Message = "Cuenta con nombre de usuario '" + account.Username + "' creada exitosamente!";
                            Thread.Sleep(5000);
                            return RedirectToAction("");
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account account)
        {
            using (DaliCMSContext db = new DaliCMSContext())
            {
                var user = db.Accounts.FirstOrDefault(u => (u.Username == account.Username));
                if (user != null)
                {
                    if(Crypto.VerifyHashedPassword(user.Password, account.Password))
                    {
                        Session["UserID"] = user.Id.ToString();
                        Session["Username"] = user.Username.ToString();
                        return RedirectToAction("");
                    }
                    else
                    {
                        ModelState.AddModelError("Login", "> Usuario y/o contraseña incorrectos");
                    }
                }
                else
                {
                    ModelState.AddModelError("Login", "> Usuario y/o contraseña incorrectos");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["userId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        
        public ActionResult Logout()
        {
            return View();
        }
    }
}