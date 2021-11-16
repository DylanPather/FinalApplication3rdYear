namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportCallouts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalloutReportCarts",
                c => new
                    {
                        RCID = c.Int(nullable: false, identity: true),
                        VehicleID = c.String(),
                        CalloutServiceID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.RCID)
                .ForeignKey("dbo.CalloutServices", t => t.CalloutServiceID, cascadeDelete: false)
                .Index(t => t.CalloutServiceID);
            
            CreateTable(
                "dbo.CalloutReportDetailTbls",
                c => new
                    {
                        CalloutReportDetailID = c.Int(nullable: false, identity: true),
                        CalloutReportID = c.Int(nullable: false),
                        CalloutServiceID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        VAT = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CalloutReportDetailID)
                .ForeignKey("dbo.CalloutReports", t => t.CalloutReportID, cascadeDelete: false)
                .ForeignKey("dbo.CalloutServices", t => t.CalloutServiceID, cascadeDelete: false)
                .Index(t => t.CalloutReportID)
                .Index(t => t.CalloutServiceID);
            
            CreateTable(
                "dbo.CalloutReports",
                c => new
                    {
                        CalloutReportID = c.Int(nullable: false, identity: true),
                        VehicleID = c.Int(nullable: false),
                        MechanicID = c.Int(nullable: false),
                        DateOfReport = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.CalloutReportID)
                .ForeignKey("dbo.MechanicTbls", t => t.MechanicID, cascadeDelete: false)
                .ForeignKey("dbo.VehicleTbls", t => t.VehicleID, cascadeDelete: false)
                .Index(t => t.VehicleID)
                .Index(t => t.MechanicID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CalloutReportDetailTbls", "CalloutServiceID", "dbo.CalloutServices");
            DropForeignKey("dbo.CalloutReportDetailTbls", "CalloutReportID", "dbo.CalloutReports");
            DropForeignKey("dbo.CalloutReports", "VehicleID", "dbo.VehicleTbls");
            DropForeignKey("dbo.CalloutReports", "MechanicID", "dbo.MechanicTbls");
            DropForeignKey("dbo.CalloutReportCarts", "CalloutServiceID", "dbo.CalloutServices");
            DropIndex("dbo.CalloutReports", new[] { "MechanicID" });
            DropIndex("dbo.CalloutReports", new[] { "VehicleID" });
            DropIndex("dbo.CalloutReportDetailTbls", new[] { "CalloutServiceID" });
            DropIndex("dbo.CalloutReportDetailTbls", new[] { "CalloutReportID" });
            DropIndex("dbo.CalloutReportCarts", new[] { "CalloutServiceID" });
            DropTable("dbo.CalloutReports");
            DropTable("dbo.CalloutReportDetailTbls");
            DropTable("dbo.CalloutReportCarts");
        }
    }
}
