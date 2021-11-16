namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orderdetailcheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PayFastShippings", "OrderDetailsTbl_OrderDetailID", c => c.Int());
            CreateIndex("dbo.PayFastShippings", "OrderDetailsTbl_OrderDetailID");
            AddForeignKey("dbo.PayFastShippings", "OrderDetailsTbl_OrderDetailID", "dbo.OrderDetailsTbls", "OrderDetailID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayFastShippings", "OrderDetailsTbl_OrderDetailID", "dbo.OrderDetailsTbls");
            DropIndex("dbo.PayFastShippings", new[] { "OrderDetailsTbl_OrderDetailID" });
            DropColumn("dbo.PayFastShippings", "OrderDetailsTbl_OrderDetailID");
        }
    }
}
