using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerceApp7.Models;

namespace ECommerceApp7.ViewModels
{
    public class SelectItemToDeleteViewModel
    {
        public List<Item> Items { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public List<Category> Categories { get; set; }
    }
}