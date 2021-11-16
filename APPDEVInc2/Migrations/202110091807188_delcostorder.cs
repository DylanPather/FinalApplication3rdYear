namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delcostorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdersTbls", "DeliveryCost", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdersTbls", "DeliveryCost");
        }
    }
}
