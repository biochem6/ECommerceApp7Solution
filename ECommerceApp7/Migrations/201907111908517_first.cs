namespace ECommerceApp7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        SubCategoryId = c.Int(nullable: false, identity: true),
                        SubCategoryName = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubCategoryId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.SubCategoryItems",
                c => new
                    {
                        SubCategoryItemsId = c.Int(nullable: false, identity: true),
                        SubcategoryId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubCategoryItemsId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.SubCategories", t => t.SubcategoryId, cascadeDelete: true)
                .Index(t => t.SubcategoryId)
                .Index(t => t.ItemId);
            
            AddColumn("dbo.Categories", "FilterButtonName", c => c.String());
            AddColumn("dbo.Items", "DateAdded", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Items", "Description", c => c.String(maxLength: 144));
            DropColumn("dbo.Items", "SkuNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "SkuNumber", c => c.Int(nullable: false));
            DropForeignKey("dbo.SubCategoryItems", "SubcategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.SubCategoryItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.SubCategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.SubCategoryItems", new[] { "ItemId" });
            DropIndex("dbo.SubCategoryItems", new[] { "SubcategoryId" });
            DropIndex("dbo.SubCategories", new[] { "CategoryId" });
            AlterColumn("dbo.Items", "Description", c => c.String(nullable: false, maxLength: 144));
            DropColumn("dbo.Items", "DateAdded");
            DropColumn("dbo.Categories", "FilterButtonName");
            DropTable("dbo.SubCategoryItems");
            DropTable("dbo.SubCategories");
        }
    }
}
