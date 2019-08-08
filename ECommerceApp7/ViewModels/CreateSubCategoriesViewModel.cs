using System.ComponentModel.DataAnnotations;

namespace ECommerceApp7.ViewModels
{
    public class CreateSubCategoriesViewModel
    {
        [Required]
        [Display(Name = "Sub-Category Name")]
        public string SubCategoryName { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public string ValidationMessage { get; set; }
    }
}