using System.Data.Entity;

namespace ECommerceApp7.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base()
        {
            //Database.SetInitializer<StoreContext>(new StoreDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}