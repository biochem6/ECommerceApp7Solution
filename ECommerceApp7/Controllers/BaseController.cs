using ECommerceApp7.Models;
using System.Linq;
using System.Web.Mvc;

namespace ECommerceApp7.Controllers
{
    public class BaseController : Controller
    {
        private readonly StoreContext _db = new StoreContext();

        public BaseController()
        {
            ViewBag.categories = _db.Categories;

        }



    }
}