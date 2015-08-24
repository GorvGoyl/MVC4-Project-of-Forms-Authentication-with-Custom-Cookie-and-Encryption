using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc4Project.Models;
using System.Web.Security;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Mvc4Project.Providers;
namespace Mvc4Project.Controllers
{
    public class AccountController : Controller
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            Logger.Debug("Method Start");

            Logger.Debug("Method End");
            return View();
        }

        public ActionResult Register()
        {
            Logger.Debug("Method Start");

            Logger.Debug("Method End");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            Logger.Debug("Method Start");


            if (ModelState.IsValid)
            {
                List<string> User = new List<string>();
                User.Add(user.UserName);
                User.Add(user.Password);
                User.Add(user.Phone);
                User.Add(user.City);
                string JsonUser = JsonConvert.SerializeObject(User, Formatting.Indented);
                BusinessAccessLayer.BusinessAccessLayer.AddUser(JsonUser);
                ViewBag.Message = "Successfully Registration Done";

            }
            Logger.Debug("Method End");
            return View(user);
        }

        public ActionResult Login()
        {
            Logger.Debug("Method Start");

            Logger.Debug("Method End");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login, string ReturnUrl = "")
        {
            Logger.Debug("Method Start");


            if (ModelState.IsValid)
            {
                string UserData = BusinessAccessLayer.BusinessAccessLayer.ValidateUser(login.Username, login.Password);
                if (UserData != "")
                {
                    HttpCookie CustomAuthCookie = new HttpCookie("MXGourav");
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(
                       1,
                       login.Username,
                       DateTime.Now,
                       DateTime.Now.AddMinutes(30),
                       login.RememberMe,
                       UserData,
                       CustomAuthCookie.Path);

                    string EncTicket = FormsAuthentication.Encrypt(Ticket);
                    Response.Cookies.Add(new HttpCookie(CustomAuthCookie.Name, EncTicket));



                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("MyProfile", "Home");
                    }
                }
            }
            ViewBag.Message = "username or password is incorrect";
            ModelState.Remove("Password");

            Logger.Debug("Method End");

            return View();
        }


        [CustomAuthorize]
        public ActionResult Logout()
        {
            Logger.Debug("Method Start");
            HttpContext.Response.Cookies.Remove("MXGourav");
            HttpContext.Response.Cookies["MXGourav"].Value = null;
            //Clearing the cookies of the response doesn't instruct the
            //browser to clear the cookie, it merely does not send the cookie back to the browser.
            //To instruct the browser to clear the cookie you need to tell it the cookie has expired
            HttpContext.Response.Cookies["MXGourav"].Expires = DateTime.Now.AddMonths(-1);

            return RedirectToAction("Login", "Account");
        }

    }
}
