using ECommerceApp7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Http;

namespace ECommerceApp7.Controllers.Api
{
    public class GetCategoryAndSubCategoriesController : ApiController
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }


        public GetCategoryAndSubCategoriesController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public IHttpActionResult GetCategoryAndSubCategories(string catName)
        {
            var getCatName = ApplicationDbContext.Categories.Where(i => i.CategoryName == catName).Select(i => i.CategoryName);

            var getCatId = ApplicationDbContext.Categories.Where(i => i.CategoryName == catName)
                .Select(i => i.CategoryId).FirstOrDefault();

            var getSubCats = ApplicationDbContext.SubCategories.Where(i => i.CategoryId == getCatId).Select(i => i.SubCategoryName);

            var catAndSubCats = new { cat = getCatName, subCats = getSubCats };

            return Ok(catAndSubCats);
        }
    }
}
