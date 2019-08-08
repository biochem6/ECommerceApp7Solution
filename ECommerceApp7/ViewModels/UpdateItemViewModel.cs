using ECommerceApp7.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp7.ViewModels
{
    public class UpdateItemViewModel
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        [StringLength(144, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageReference { get; set; }

        public List<Category> Categories { get; set; }

        [Display(Name = "Category: ")]
        public string CategoryName { get; set; }

        public DateTime DateAdded { get; set; }

        public IEnumerable<int> Subcategories { get; set; }
    }
}