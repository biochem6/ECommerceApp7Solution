using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp7.ViewModels
{
    public class CreateItemViewModel
    {


        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }


        [StringLength(144, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Image file name. eg: shoes.jpg")]
        public string ImageReference { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<string> SubCategories { get; set; }

        public int SubCategoryId { get; set; }

        public int ItemId { get; set; }





    }
}