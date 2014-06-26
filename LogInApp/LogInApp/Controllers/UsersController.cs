using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;

namespace LogInApp.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LogInApp.Models.UserLogin user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(LogInApp.Models.UserDetail user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new LogInApp.Models.UserEntities())
                    {
                        var crypto = new SimpleCrypto.PBKDF2();
                        var encrppwd = crypto.Compute(user.Password);
                        var sysuser = db.UserDetails.Create();

                        //Random r=new Random();
                        sysuser.Id = user.Id;
                        sysuser.UserName = user.UserName;
                        sysuser.Password = encrppwd;
                        sysuser.EmailId = crypto.Salt;
                        sysuser.PhoneNo = user.PhoneNo;

                        //TryUpdateModel(sysuser);
                        db.UserDetails.Add(sysuser);
                        db.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            { 
            }
            return View();
        }

        public ActionResult LogOut()
        {
            return View();
        }

        private bool IsValid(string uname, string pwd)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            bool isvalid = false;
            using (var db = new LogInApp.Models.UserEntities())
            {
                var user = db.UserDetails.FirstOrDefault(u => u.UserName == uname);
                if (user != null)
                {
                    if (user.Password == crypto.Compute(pwd, user.EmailId))
                    {
                        isvalid = true;
                    }
                }
            }

            return isvalid;
        }
    }
}
