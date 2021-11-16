namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Toolsandcallout : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalloutTbls",
                c => new
                    {
                        CalloutID = c.Int(nullable: false, identity: true),
                        RequestID = c.Int(nullable: false),
                        MechanicID = c.Int(nullable: false),
                        IsEnRoute = c.Boolean(nullable: false),
                        IsArrived = c.Boolean(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.CalloutID)
                .ForeignKey("dbo.MechanicTbls", t => t.MechanicID, cascadeDelete: false)
                .ForeignKey("dbo.RequestAssistanceTbls", t => t.RequestID, cascadeDelete: false)
                .Index(t => t.RequestID)
                .Index(t => t.MechanicID);
            
            CreateTable(
                "dbo.ToolBoxTbls",
                c => new
                    {
                        ToolBoxID = c.Int(nullable: false, identity: true),
                        ToolBoxColor = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ToolBoxID);
            
            CreateTable(
                "dbo.ToolsTbls",
                c => new
                    {
                        ToolID = c.Int(nullable: false, identity: true),
                        ToolBoxID = c.Int(nullable: false),
                        ToolName = c.String(),
                        ToolBrand = c.String(),
                        ToolCost = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        IsMissing = c.Boolean(nullable: false),
                        IsDamaged = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ToolID)
                .ForeignKey("dbo.ToolBoxTbls", t => t.ToolBoxID, cascadeDelete: false)
                .Index(t => t.ToolBoxID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToolsTbls", "ToolBoxID", "dbo.ToolBoxTbls");
            DropForeignKey("dbo.CalloutTbls", "RequestID", "dbo.RequestAssistanceTbls");
            DropForeignKey("dbo.CalloutTbls", "MechanicID", "dbo.MechanicTbls");
            DropIndex("dbo.ToolsTbls", new[] { "ToolBoxID" });
            DropIndex("dbo.CalloutTbls", new[] { "MechanicID" });
            DropIndex("dbo.CalloutTbls", new[] { "RequestID" });
            DropTable("dbo.ToolsTbls");
            DropTable("dbo.ToolBoxTbls");
            DropTable("dbo.CalloutTbls");
        }
    }
}
