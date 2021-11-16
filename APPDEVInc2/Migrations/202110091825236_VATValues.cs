namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VATValues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetailsTbls", "VAT", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrdersTbls", "VAT", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdersTbls", "VAT");
            DropColumn("dbo.OrderDetailsTbls", "VAT");
        }
    }
}
