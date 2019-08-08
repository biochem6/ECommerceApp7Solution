using ECommerceApp7.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace ECommerceApp7.Controllers
{
    public class HomeController : BaseController
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public ActionResult Index()
        {

            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return View();
            }

            var name = System.Web.HttpContext.Current.User.Identity.GetUserName();
            if (name == "admin_789@gmail.com")
            {
                return RedirectToAction("Home", "Admin");
            }

            return View();


        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}