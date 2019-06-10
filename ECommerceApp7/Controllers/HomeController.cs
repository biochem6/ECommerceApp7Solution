using ECommerceApp7.Models;
using System.Web.Mvc;

namespace ECommerceApp7.Controllers
{
    public class HomeController : BaseController
    {
       // private readonly ItemDbContext _db = new ItemDbContext();

        public ActionResult Index()
        {

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