using System.Collections.Generic;

namespace ECommerceApp7.ViewModels
{
    public class UpdateCategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<string> SubCategories { get; set; }

        public string FilterButtonName { get; set; }

        public string SubcategoryName { get; set; }
    }
}