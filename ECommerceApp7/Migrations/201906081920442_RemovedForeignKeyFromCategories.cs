namespace ECommerceApp7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedForeignKeyFromCategories : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Categories", "ItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "ItemId", c => c.Int(nullable: false));
        }
    }
}
