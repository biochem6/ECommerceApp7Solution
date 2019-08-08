using ECommerceApp7.Models;
using System.Linq;
using System.Web.Mvc;

namespace ECommerceApp7.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public BaseController()
        {

            ViewBag.categories = _db.Categories.ToList();

        }



    }
}