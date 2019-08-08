using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp7.Models
{
    public sealed class Item
    {
        public Item()
        {
            Categories = new HashSet<Category>();
        }

        public int ItemId { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }

        public bool IsOnSale { get; set; }

        public int SalePercent { get; set; }

        [StringLength(144, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageReference { get; set; }

        public ICollection<Category> Categories { get; set; }

        public int CategoryId { get; set; }

        public DateTime DateAdded { get; set; }



    }

}