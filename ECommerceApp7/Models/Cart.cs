using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerceApp7.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public ApplicationUser UserId { get; set;  }

        public int ItemQuantity { get; set; }

        public decimal Price { get; set; }

        public string ItemName { get; set; }

        public int SkuNumber { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public DateTime DateAdded { get; set; }


    }
}