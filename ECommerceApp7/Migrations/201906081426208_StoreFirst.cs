namespace ECommerceApp7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreFirst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsOnSale = c.Boolean(nullable: false),
                        SalePercent = c.Int(nullable: false),
                        Description = c.String(),
                        SkuNumber = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageReference = c.String(),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemCategories",
                c => new
                    {
                        Item_ItemId = c.Int(nullable: false),
                        Category_CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_ItemId, t.Category_CategoryId })
                .ForeignKey("dbo.Items", t => t.Item_ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId, cascadeDelete: true)
                .Index(t => t.Item_ItemId)
                .Index(t => t.Category_CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemCategories", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ItemCategories", "Item_ItemId", "dbo.Items");
            DropIndex("dbo.ItemCategories", new[] { "Category_CategoryId" });
            DropIndex("dbo.ItemCategories", new[] { "Item_ItemId" });
            DropTable("dbo.ItemCategories");
            DropTable("dbo.Items");
            DropTable("dbo.Categories");
        }
    }
}
