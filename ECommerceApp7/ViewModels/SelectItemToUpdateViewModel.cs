using ECommerceApp7.Models;
using System;
using System.Collections.Generic;

namespace ECommerceApp7.ViewModels
{
    public class SelectItemToUpdateViewModel
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
        public string ImageReference { get; set; }
        public string Description { get; set; }

        public List<Item> Items { get; set; }
        public int CategoryId { get; set; }
        public List<Category> CategoryName { get; set; }
    }
}