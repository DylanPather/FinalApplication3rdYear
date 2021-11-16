namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Quotations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuotationDetailTbls",
                c => new
                    {
                        QuoteDetailID = c.Int(nullable: false, identity: true),
                        QuotationID = c.Int(nullable: false),
                        StockID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.QuoteDetailID);
            
            CreateTable(
                "dbo.QuotationTbls",
                c => new
                    {
                        QuotationID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        QuoteDate = c.DateTime(),
                        QuoteTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateModified = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.QuotationID)
                .ForeignKey("dbo.CustomerTbls", t => t.CustomerID, cascadeDelete: false)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.QuoteCarts",
                c => new
                    {
                        CID = c.Int(nullable: false, identity: true),
                        CartID = c.String(),
                        StockID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CID)
                .ForeignKey("dbo.StockTbls", t => t.StockID, cascadeDelete: false)
                .Index(t => t.StockID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuoteCarts", "StockID", "dbo.StockTbls");
            DropForeignKey("dbo.QuotationTbls", "CustomerID", "dbo.CustomerTbls");
            DropIndex("dbo.QuoteCarts", new[] { "StockID" });
            DropIndex("dbo.QuotationTbls", new[] { "CustomerID" });
            DropTable("dbo.QuoteCarts");
            DropTable("dbo.QuotationTbls");
            DropTable("dbo.QuotationDetailTbls");
        }
    }
}
