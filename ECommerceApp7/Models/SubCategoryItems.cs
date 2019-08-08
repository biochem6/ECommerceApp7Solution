namespace ECommerceApp7.Models
{
    public class SubCategoryItems
    {
        public int SubCategoryItemsId { get; set; }

        public SubCategory SubCategory { get; set; }
        public int SubcategoryId { get; set; }

        public Item Item { get; set; }
        public int ItemId { get; set; }

    }
}