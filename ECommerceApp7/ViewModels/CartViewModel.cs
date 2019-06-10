using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerceApp7.Models;
using ECommerceApp7.Controllers;

namespace ECommerceApp7.ViewModels
{
    public class CartViewModel 
    {
        public int ID { get; set; }

        public ApplicationUser Shopper { get; set; }

        public string ShopperId { get; set; }
        

        public decimal Price { get; set; }

        public string Name { get; set; }

        public int SKU_Number { get; set; }
    }
}