namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Post : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.PayFastShippings", "OrderID", "dbo.OrderDetailsTbls", "OrderDetailID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayFastShippings", "OrderID", "dbo.OrderDetailsTbls");
        }
    }
}
