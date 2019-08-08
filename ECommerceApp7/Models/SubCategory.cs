using System.ComponentModel.DataAnnotations;

namespace ECommerceApp7.Models
{
    public class SubCategory
    {

        public int SubCategoryId { get; set; }

        [Display(Name = "Sub-Category Name")]
        public string SubCategoryName { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

    
    }
}