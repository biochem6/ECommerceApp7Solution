using ECommerceApp7.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp7.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Filter Button Name (eg: 'Type', 'Size', 'Length')")]
        public string FilterButtonName { get; set; }

    }
}