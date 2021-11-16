namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quotelinking : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.QuotationDetailTbls", "QuotationID");
            CreateIndex("dbo.QuotationDetailTbls", "StockID");
            AddForeignKey("dbo.QuotationDetailTbls", "QuotationID", "dbo.QuotationTbls", "QuotationID", cascadeDelete: false);
            AddForeignKey("dbo.QuotationDetailTbls", "StockID", "dbo.StockTbls", "StockID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuotationDetailTbls", "StockID", "dbo.StockTbls");
            DropForeignKey("dbo.QuotationDetailTbls", "QuotationID", "dbo.QuotationTbls");
            DropIndex("dbo.QuotationDetailTbls", new[] { "StockID" });
            DropIndex("dbo.QuotationDetailTbls", new[] { "QuotationID" });
        }
    }
}
