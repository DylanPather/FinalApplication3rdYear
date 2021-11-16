namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cash_cardPaymentincashuptable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CashUpHistories", "TotalCashPayments", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CashUpHistories", "TotalCardPayments", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CashUpHistories", "TotalCardPayments");
            DropColumn("dbo.CashUpHistories", "TotalCashPayments");
        }
    }
}
