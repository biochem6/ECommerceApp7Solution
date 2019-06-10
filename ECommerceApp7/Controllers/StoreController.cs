using ECommerceApp7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        private StoreContext db = new StoreContext();

        // GET: Store
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        // GET: Store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
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

        // POST: Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,Name,IsOnSale,SalePercent,Description,SkuNumber,Price,ImageReference")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,Name,IsOnSale,SalePercent,Description,SkuNumber,Price,ImageReference")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Store/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Category(int id)
        {
            IEnumerable<Item> getCategory = db.Items.Where(i => i.CategoryId == id);

            return View(getCategory);
        }

        public ActionResult Sale()
        {
            IEnumerable<Item> getSaleItems = db.Items.Where(i => i.IsOnSale == true);

            return View(getSaleItems);
        }


        public ActionResult Cart()
        {
            return View();
        }

        //Add to Cart for logged in user
        [HttpPost]
        [Authorize]
        public ActionResult Cart(string user, int itemId)
        {

            IQueryable<Item> itemAdded = db.Items.Where(i => i.ItemId == itemId);

            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = ApplicationDbContext.Users.FirstOrDefault(i => i.Id == userId);


            string itemName = itemAdded.Where(i => i.ItemId == itemId).Select(j => j.Name).ToString();
    
            string itemCategory = itemAdded.Select(i => i.Categories.Where(j => j.CategoryId == i.CategoryId).Select(k => k.CategoryName)).ToString();

            int skuNumber = itemAdded.Where(i => i.ItemId == itemId).Select(j => j.SkuNumber).FirstOrDefault();

            decimal itemPrice = itemAdded.Where(i => i.ItemId == itemId).Select(j => j.Price).FirstOrDefault();

            int categoryId = itemAdded.Where(i => i.ItemId == itemId).Select(j => j.CategoryId).FirstOrDefault();

            DateTime timeAdded = DateTime.Now;

            IList<Cart> newCart = new List<Cart>() { new Cart() { UserId = currentUser, CategoryName = itemCategory, ItemName = itemName, SkuNumber = skuNumber, DateAdded = timeAdded, Price = itemPrice, ItemQuantity = 1, CategoryId = categoryId} };
            db.Carts.AddRange(newCart);
            db.SaveChanges();

            return View();


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
