namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToolsCheckInCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToolsCheckInCarts",
                c => new
                    {
                        ToolCheckInID = c.Int(nullable: false, identity: true),
                        ToolBoxID = c.Int(nullable: false),
                        ToolID = c.Int(nullable: false),
                        IsPresent = c.Boolean(nullable: false),
                        IsMissing = c.Boolean(nullable: false),
                        IsDamaged = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ToolCheckInID)
                .ForeignKey("dbo.ToolBoxTbls", t => t.ToolBoxID, cascadeDelete: false)
                .ForeignKey("dbo.ToolsTbls", t => t.ToolID, cascadeDelete: false)
                .Index(t => t.ToolBoxID)
                .Index(t => t.ToolID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToolsCheckInCarts", "ToolID", "dbo.ToolsTbls");
            DropForeignKey("dbo.ToolsCheckInCarts", "ToolBoxID", "dbo.ToolBoxTbls");
            DropIndex("dbo.ToolsCheckInCarts", new[] { "ToolID" });
            DropIndex("dbo.ToolsCheckInCarts", new[] { "ToolBoxID" });
            DropTable("dbo.ToolsCheckInCarts");
        }
    }
}
