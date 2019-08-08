using ECommerceApp7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ECommerceApp7.Controllers.Api
{
    [System.Web.Mvc.Authorize(Users = "admin_789@gmail.com")]
    public class GetItemSubcategoryController : ApiController
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }


        public GetItemSubcategoryController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

       
        public IHttpActionResult GetItemSubcategory(int itemId)
        {
            IEnumerable<int> itemSubcategoryIds = ApplicationDbContext.SubCategoryItems
                                                                      .Where(i => i.ItemId == itemId)
                                                                      .Select(i => i.SubcategoryId)
                                                                      .ToList();
            List<string> itemSubcategories = new List<string>();
            foreach (var subcatid in itemSubcategoryIds)
            {
                itemSubcategories.AddRange(ApplicationDbContext.SubCategories
                                                          .Where(i => i.SubCategoryId == subcatid)
                                                          .Select(i => i.SubCategoryName));

            }

            return Ok(itemSubcategories);
        }
    }
}
