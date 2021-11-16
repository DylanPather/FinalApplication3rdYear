namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class priceinorderdetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetailsTbls", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetailsTbls", "Price");
        }
    }
}
