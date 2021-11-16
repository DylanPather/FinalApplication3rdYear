namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceTbls",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                        AmountPaid = c.Decimal(precision: 18, scale: 2),
                        DateOfInvoice = c.DateTime(),
                        FromReport = c.Boolean(nullable: false),
                        FromQuotation = c.Boolean(nullable: false),
                        PaymentType = c.String(),
                    })
                .PrimaryKey(t => t.InvoiceID)
                .ForeignKey("dbo.CustomerTbls", t => t.CustomerID, cascadeDelete: false)
                .ForeignKey("dbo.VehicleTbls", t => t.VehicleID, cascadeDelete: false)
                .Index(t => t.CustomerID)
                .Index(t => t.VehicleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceTbls", "VehicleID", "dbo.VehicleTbls");
            DropForeignKey("dbo.InvoiceTbls", "CustomerID", "dbo.CustomerTbls");
            DropIndex("dbo.InvoiceTbls", new[] { "VehicleID" });
            DropIndex("dbo.InvoiceTbls", new[] { "CustomerID" });
            DropTable("dbo.InvoiceTbls");
        }
    }
}
