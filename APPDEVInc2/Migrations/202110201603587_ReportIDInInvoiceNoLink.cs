namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportIDInInvoiceNoLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceTbls", "ReportID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceTbls", "ReportID");
        }
    }
}
