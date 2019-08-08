using System;
using System.Collections.Generic;
using ECommerceApp7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Http;

namespace ECommerceApp7.Controllers.Api
{
    public class GetSubCategoriesController : ApiController
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }


        public GetSubCategoriesController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public IHttpActionResult GetSubCategories(string categoryName)
        {
            int getCatId = ApplicationDbContext.Categories
                                               .Where(i => i.CategoryName == categoryName)
                                               .Select(i => i.CategoryId).FirstOrDefault();

            List<string> getSubcategoryNames = ApplicationDbContext.SubCategories
                                                       .Where(i => i.CategoryId == getCatId)
                                                       .Select(i => i.SubCategoryName).ToList();

            List<int> getSubcategoryIds = ApplicationDbContext.SubCategories
                                                          .Where(i => i.CategoryId == getCatId)
                                                          .Select(i => i.SubCategoryId).ToList();

            var list = getSubcategoryIds.Select((t, i) => new KeyValuePair<int, string>(t, getSubcategoryNames[i])).ToList();

            

         

            return Ok(list);
        }
    }
}
