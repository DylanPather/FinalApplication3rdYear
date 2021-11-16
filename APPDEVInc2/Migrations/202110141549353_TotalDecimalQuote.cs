namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalDecimalQuote : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuotationTbls", "QuoteTotal", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuotationTbls", "QuoteTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
