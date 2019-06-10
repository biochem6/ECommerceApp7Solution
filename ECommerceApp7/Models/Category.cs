using System.Collections.Generic;

namespace ECommerceApp7.Models
{
    public sealed class Category
    {
        public Category()
        {
            this.Items = new HashSet<Item>();
        }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public ICollection<Item> Items { get; set; }

    }


}