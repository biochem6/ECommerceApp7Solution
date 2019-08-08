using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp7.Models
{
    public sealed class Category : IEnumerable
    {
        public Category()
        {
            this.Items = new HashSet<Item>();
            this.SubCategories = new HashSet<SubCategory>();
        }

        public ICollection<Item> Items { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Filter Button Name")]
        public string FilterButtonName { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }

}
