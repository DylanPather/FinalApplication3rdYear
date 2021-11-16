namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temptbltest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportQuoteTempTbls", "QuotationID", c => c.Int(nullable: false));
            AddColumn("dbo.ReportQuoteTempTbls", "ReportID", c => c.Int(nullable: false));
            CreateIndex("dbo.ReportQuoteTempTbls", "QuotationID");
            CreateIndex("dbo.ReportQuoteTempTbls", "ReportID");
            AddForeignKey("dbo.ReportQuoteTempTbls", "QuotationID", "dbo.QuotationTbls", "QuotationID", cascadeDelete: false);
            AddForeignKey("dbo.ReportQuoteTempTbls", "ReportID", "dbo.ReportTbls", "ReportID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportQuoteTempTbls", "ReportID", "dbo.ReportTbls");
            DropForeignKey("dbo.ReportQuoteTempTbls", "QuotationID", "dbo.QuotationTbls");
            DropIndex("dbo.ReportQuoteTempTbls", new[] { "ReportID" });
            DropIndex("dbo.ReportQuoteTempTbls", new[] { "QuotationID" });
            DropColumn("dbo.ReportQuoteTempTbls", "ReportID");
            DropColumn("dbo.ReportQuoteTempTbls", "QuotationID");
        }
    }
}
