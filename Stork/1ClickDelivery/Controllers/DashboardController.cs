using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using _1ClickDelivery.Models;
using System.Web.Security;

namespace _1ClickDelivery.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var id = User.Identity.GetUserId();
                var user = db.Users.SingleOrDefault(x => x.Id == id);
                ViewData["AccountInfo"] = user;
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "AdminDashboard");
                else
                    return View();

            }

            
        }

        public ActionResult _MenuPartial()
        {
            return PartialView();
        }


    }
}