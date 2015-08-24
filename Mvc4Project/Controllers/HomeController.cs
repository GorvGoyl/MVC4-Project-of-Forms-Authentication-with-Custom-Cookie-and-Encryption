using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Mvc4Project.Providers;
using System.Web.Security;
using Newtonsoft.Json;
namespace Mvc4Project.Controllers
{
    public class HomeController : Controller
    {
        //private static log4net.ILog Log { get; set; }
        //ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [AllowAnonymous] //This is for Un-Authorize User
        public ActionResult Index()
        {
            Logger.Debug("Method Start");

            Logger.Debug("Method End");
            return View();
        }

        [CustomAuthorize] // This is for Authorize user
        public ActionResult MyProfile()
        {
            Logger.Debug("Method Start");
            if (HttpContext.Request.Cookies["MXGourav"] != null)
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get("MXGourav");
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                string Json = ticket.UserData;
                List<string> UserData = JsonConvert.DeserializeObject<List<string>>(Json);
                ViewBag.UserName = UserData[0];
                // ViewBag.Name = UserData[1];
                ViewBag.Phone = UserData[2];
                ViewBag.City = UserData[3];
            }
            Logger.Debug("Method End");
            return View();
        }
    }
}
