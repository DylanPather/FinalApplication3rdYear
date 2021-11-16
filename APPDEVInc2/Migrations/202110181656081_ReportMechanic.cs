namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportMechanic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportCarts",
                c => new
                    {
                        RCID = c.Int(nullable: false, identity: true),
                        VehicleID = c.Int(nullable: false),
                        StockID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.RCID)
                .ForeignKey("dbo.StockTbls", t => t.StockID, cascadeDelete: false)
                .Index(t => t.StockID);
            
            CreateTable(
                "dbo.ReportTbls",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        VehicleID = c.Int(nullable: false),
                        MechanicID = c.Int(nullable: false),
                        DateOfReport = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ReportID)
                .ForeignKey("dbo.MechanicTbls", t => t.MechanicID, cascadeDelete: false)
                .ForeignKey("dbo.VehicleTbls", t => t.VehicleID, cascadeDelete: false)
                .Index(t => t.VehicleID)
                .Index(t => t.MechanicID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportTbls", "VehicleID", "dbo.VehicleTbls");
            DropForeignKey("dbo.ReportTbls", "MechanicID", "dbo.MechanicTbls");
            DropForeignKey("dbo.ReportCarts", "StockID", "dbo.StockTbls");
            DropIndex("dbo.ReportTbls", new[] { "MechanicID" });
            DropIndex("dbo.ReportTbls", new[] { "VehicleID" });
            DropIndex("dbo.ReportCarts", new[] { "StockID" });
            DropTable("dbo.ReportTbls");
            DropTable("dbo.ReportCarts");
        }
    }
}
