namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuantityStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockTbls", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockTbls", "Quantity");
        }
    }
}
