namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cashuphistorytbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CashUpHistories",
                c => new
                    {
                        CashUpID = c.Int(nullable: false, identity: true),
                        CashUpDate = c.DateTime(),
                        OnlineSales = c.Decimal(precision: 18, scale: 2),
                        InStoreSales = c.Decimal(precision: 18, scale: 2),
                        DailyExpense = c.Decimal(precision: 18, scale: 2),
                        TillFloat = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CashUpID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CashUpHistories");
        }
    }
}
