using ECommerceApp7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ECommerceApp7.Controllers
{
    public class StoreController : BaseController
    {

        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }


        public StoreController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }


        // GET: Store
        public ActionResult Index()
        {
            return View(ApplicationDbContext.Items.ToList());
        }

        // GET: Store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = ApplicationDbContext.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Category(string categoryName)
        {
            Category getCategoryId = ApplicationDbContext.Categories?
                                                    .First(i => i.CategoryName == categoryName);


            IEnumerable<Item> getCategory = ApplicationDbContext.Items
                                                                .Where(i => i.CategoryId == getCategoryId.CategoryId);


            return View(getCategory);
        }


        public ActionResult Sale()
        {
            IEnumerable<Item> getSaleItems = ApplicationDbContext.Items.Where(i => i.IsOnSale == true);

            return View(getSaleItems);
        }


        [HttpPost]
        public ActionResult AddItemToCart(int id)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = ApplicationDbContext.Users.FirstOrDefault(i => i.Id == userId);


            List<string> itemName = ApplicationDbContext.Items
                .Where(i => i.ItemId == id)
                .Select(i => i.Name).ToList();

            int categoryId = ApplicationDbContext.Items
                .Where(i => i.ItemId == id)
                .Select(i => i.CategoryId)
                .FirstOrDefault();

            List<string> itemCategory = ApplicationDbContext.Categories
                .Where(i => i.CategoryId == categoryId)
                .Select(i => i.CategoryName).ToList();


            decimal itemPrice = ApplicationDbContext.Items
                .Where(i => i.ItemId == id)
                .Select(j => j.Price)
                .FirstOrDefault();


            DateTime timeAdded = DateTime.Now;


            //If user is logged in the cart will be saved in user's cart db as well as in session.
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var newCart = new Cart
                {
                    UserId = currentUser,
                    CategoryName = itemCategory[0],
                    ItemName = itemName[0],
                    DateAdded = timeAdded,
                    Price = itemPrice,
                    ItemQuantity = 1,
                    CategoryId = categoryId

                };

                ApplicationDbContext.Carts.Add(newCart);
                ApplicationDbContext.SaveChanges();

                return null;
            }

            if (Session["cart"] == null)
            {

                List<Cart> cart = new List<Cart>
                {
                    new Cart
                    {
                        UserId = currentUser,
                        CategoryName = itemCategory[0],
                        ItemName = itemName[0],
                        DateAdded = timeAdded,
                        Price = itemPrice,
                        ItemQuantity = 1,
                        CategoryId = categoryId
                    }
                };
                Session["cart"] = cart;
            }

            return null;
        }

        public ActionResult Cart()
        {
            string userId = User.Identity.GetUserId();

            IQueryable<Cart> cart = ApplicationDbContext.Carts
                                           .Where(i => i.UserId.Id == userId);
            return View(cart);
        }


        public ActionResult Search(string searchTerm)
        {
            if (searchTerm == null)
            {
                return null;
            }

            IQueryable<Item> searchItems = ApplicationDbContext.Items.Where(i => i.Name.Contains(searchTerm));

            return View(searchItems);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ApplicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
