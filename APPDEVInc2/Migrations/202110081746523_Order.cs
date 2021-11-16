namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdersTbls", "DateOfOrder", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrdersTbls", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdersTbls", "Is_Delete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdersTbls", "Is_Delete");
            DropColumn("dbo.OrdersTbls", "Is_Active");
            DropColumn("dbo.OrdersTbls", "DateOfOrder");
        }
    }
}
