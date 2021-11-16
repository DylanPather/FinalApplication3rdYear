namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeorderdetailfrompayfast : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PayFastShippings", "OrderID", "dbo.OrderDetailsTbls");
        }
        
        public override void Down()
        {
            AddForeignKey("dbo.PayFastShippings", "OrderID", "dbo.OrderDetailsTbls", "OrderDetailID", cascadeDelete: false);
        }
    }
}
