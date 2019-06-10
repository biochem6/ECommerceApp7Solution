namespace ECommerceApp7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ItemId", c => c.Int(nullable: false));
            AddColumn("dbo.Items", "CategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "CategoryId");
            DropColumn("dbo.Categories", "ItemId");
        }
    }
}
