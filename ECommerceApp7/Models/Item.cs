using System.Collections.Generic;

namespace ECommerceApp7.Models
{
    public sealed class Item
    {
        public Item()
        {
            this.Categories = new HashSet<Category>();
        }

        public int ItemId { get; set; }

        public string Name { get; set; }

        public bool IsOnSale { get; set; }

        public int SalePercent { get; set; }

        public string Description { get; set; }

        public int SkuNumber { get; set; }

        public decimal Price { get; set; }

        public string ImageReference { get; set; }

        public ICollection<Category> Categories { get; set; }

        public int CategoryId { get; set; }


    }


}