namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceCalloutBl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceCalloutTbls",
                c => new
                    {
                        InvoiceCalloutID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                        AmountPaid = c.Decimal(precision: 18, scale: 2),
                        DateOfInvoice = c.DateTime(),
                        PaymentType = c.String(),
                        CalloutReportID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceCalloutID)
                .ForeignKey("dbo.CalloutReports", t => t.CalloutReportID, cascadeDelete: false)
                .ForeignKey("dbo.CustomerTbls", t => t.CustomerID, cascadeDelete: false)
                .ForeignKey("dbo.VehicleTbls", t => t.VehicleID, cascadeDelete: false)
                .Index(t => t.CustomerID)
                .Index(t => t.VehicleID)
                .Index(t => t.CalloutReportID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceCalloutTbls", "VehicleID", "dbo.VehicleTbls");
            DropForeignKey("dbo.InvoiceCalloutTbls", "CustomerID", "dbo.CustomerTbls");
            DropForeignKey("dbo.InvoiceCalloutTbls", "CalloutReportID", "dbo.CalloutReports");
            DropIndex("dbo.InvoiceCalloutTbls", new[] { "CalloutReportID" });
            DropIndex("dbo.InvoiceCalloutTbls", new[] { "VehicleID" });
            DropIndex("dbo.InvoiceCalloutTbls", new[] { "CustomerID" });
            DropTable("dbo.InvoiceCalloutTbls");
        }
    }
}
