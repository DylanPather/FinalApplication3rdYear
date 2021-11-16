namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempReportQuote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportQuoteTempTbls",
                c => new
                    {
                        ReportQuoteTempID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReportQuoteTempID)
                .ForeignKey("dbo.BookingTbls", t => t.BookingID, cascadeDelete: false)
                .Index(t => t.BookingID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportQuoteTempTbls", "BookingID", "dbo.BookingTbls");
            DropIndex("dbo.ReportQuoteTempTbls", new[] { "BookingID" });
            DropTable("dbo.ReportQuoteTempTbls");
        }
    }
}
