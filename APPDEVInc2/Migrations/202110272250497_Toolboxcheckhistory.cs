namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Toolboxcheckhistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToolBoxCheckInHistories",
                c => new
                    {
                        ToolCheckInHistoryID = c.Int(nullable: false, identity: true),
                        CalloutID = c.Int(nullable: false),
                        MechanicID = c.Int(nullable: false),
                        CostOfDamagedMissingTools = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ToolCheckInHistoryID)
                .ForeignKey("dbo.CalloutTbls", t => t.CalloutID, cascadeDelete: false)
                .ForeignKey("dbo.MechanicTbls", t => t.MechanicID, cascadeDelete: false)
                .Index(t => t.CalloutID)
                .Index(t => t.MechanicID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToolBoxCheckInHistories", "MechanicID", "dbo.MechanicTbls");
            DropForeignKey("dbo.ToolBoxCheckInHistories", "CalloutID", "dbo.CalloutTbls");
            DropIndex("dbo.ToolBoxCheckInHistories", new[] { "MechanicID" });
            DropIndex("dbo.ToolBoxCheckInHistories", new[] { "CalloutID" });
            DropTable("dbo.ToolBoxCheckInHistories");
        }
    }
}
