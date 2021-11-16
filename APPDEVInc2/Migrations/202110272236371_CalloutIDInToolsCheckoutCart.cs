namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalloutIDInToolsCheckoutCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToolsCheckInCarts", "CalloutID", c => c.Int(nullable: false));
            CreateIndex("dbo.ToolsCheckInCarts", "CalloutID");
            AddForeignKey("dbo.ToolsCheckInCarts", "CalloutID", "dbo.CalloutTbls", "CalloutID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToolsCheckInCarts", "CalloutID", "dbo.CalloutTbls");
            DropIndex("dbo.ToolsCheckInCarts", new[] { "CalloutID" });
            DropColumn("dbo.ToolsCheckInCarts", "CalloutID");
        }
    }
}
