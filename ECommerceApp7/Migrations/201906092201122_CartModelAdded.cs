namespace ECommerceApp7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ItemQuantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemName = c.String(),
                        SkuNumber = c.Int(nullable: false),
                        CategoryName = c.String(),
                        CategoryId = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Carts");
        }
    }
}
