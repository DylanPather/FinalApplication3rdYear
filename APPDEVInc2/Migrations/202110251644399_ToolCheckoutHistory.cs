namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToolCheckoutHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToolsCheckOuts",
                c => new
                    {
                        ToolBoxCheckOutID = c.Int(nullable: false, identity: true),
                        CalloutID = c.Int(nullable: false),
                        MechanicID = c.Int(nullable: false),
                        ToolBoxID = c.Int(nullable: false),
                        DateTimeCheckedOut = c.DateTime(),
                        DateTimeReturned = c.DateTime(),
                        IsCheckedOut = c.Boolean(nullable: false),
                        IsCheckedIn = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ToolBoxCheckOutID)
                .ForeignKey("dbo.CalloutTbls", t => t.CalloutID, cascadeDelete: false)
                .ForeignKey("dbo.MechanicTbls", t => t.MechanicID, cascadeDelete: false)
                .ForeignKey("dbo.ToolBoxTbls", t => t.ToolBoxID, cascadeDelete: false)
                .Index(t => t.CalloutID)
                .Index(t => t.MechanicID)
                .Index(t => t.ToolBoxID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToolsCheckOuts", "ToolBoxID", "dbo.ToolBoxTbls");
            DropForeignKey("dbo.ToolsCheckOuts", "MechanicID", "dbo.MechanicTbls");
            DropForeignKey("dbo.ToolsCheckOuts", "CalloutID", "dbo.CalloutTbls");
            DropIndex("dbo.ToolsCheckOuts", new[] { "ToolBoxID" });
            DropIndex("dbo.ToolsCheckOuts", new[] { "MechanicID" });
            DropIndex("dbo.ToolsCheckOuts", new[] { "CalloutID" });
            DropTable("dbo.ToolsCheckOuts");
        }
    }
}
