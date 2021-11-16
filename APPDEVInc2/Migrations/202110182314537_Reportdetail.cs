namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reportdetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportDetailTbls",
                c => new
                    {
                        ReportDetailID = c.Int(nullable: false, identity: true),
                        ReportID = c.Int(nullable: false),
                        StockID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        VAT = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ReportDetailID)
                .ForeignKey("dbo.ReportTbls", t => t.ReportID, cascadeDelete: false)
                .ForeignKey("dbo.StockTbls", t => t.StockID, cascadeDelete: false)
                .Index(t => t.ReportID)
                .Index(t => t.StockID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportDetailTbls", "StockID", "dbo.StockTbls");
            DropForeignKey("dbo.ReportDetailTbls", "ReportID", "dbo.ReportTbls");
            DropIndex("dbo.ReportDetailTbls", new[] { "StockID" });
            DropIndex("dbo.ReportDetailTbls", new[] { "ReportID" });
            DropTable("dbo.ReportDetailTbls");
        }
    }
}
