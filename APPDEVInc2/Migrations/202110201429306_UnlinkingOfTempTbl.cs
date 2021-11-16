namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnlinkingOfTempTbl : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReportQuoteTempTbls", "BookingID", "dbo.BookingTbls");
            DropForeignKey("dbo.ReportQuoteTempTbls", "QuotationID", "dbo.QuotationTbls");
            DropForeignKey("dbo.ReportQuoteTempTbls", "ReportID", "dbo.ReportTbls");
            DropIndex("dbo.ReportQuoteTempTbls", new[] { "BookingID" });
            DropIndex("dbo.ReportQuoteTempTbls", new[] { "QuotationID" });
            DropIndex("dbo.ReportQuoteTempTbls", new[] { "ReportID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ReportQuoteTempTbls", "ReportID");
            CreateIndex("dbo.ReportQuoteTempTbls", "QuotationID");
            CreateIndex("dbo.ReportQuoteTempTbls", "BookingID");
            AddForeignKey("dbo.ReportQuoteTempTbls", "ReportID", "dbo.ReportTbls", "ReportID", cascadeDelete: true);
            AddForeignKey("dbo.ReportQuoteTempTbls", "QuotationID", "dbo.QuotationTbls", "QuotationID", cascadeDelete: true);
            AddForeignKey("dbo.ReportQuoteTempTbls", "BookingID", "dbo.BookingTbls", "BookingID", cascadeDelete: true);
        }
    }
}
