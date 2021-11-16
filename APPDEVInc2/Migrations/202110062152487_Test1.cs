namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StockTbls", "TyreID", "dbo.TyresTbls");
            DropIndex("dbo.StockTbls", new[] { "TyreID" });
            AlterColumn("dbo.StockTbls", "TyreID", c => c.Int());
            CreateIndex("dbo.StockTbls", "TyreID");
            AddForeignKey("dbo.StockTbls", "TyreID", "dbo.TyresTbls", "TyreID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockTbls", "TyreID", "dbo.TyresTbls");
            DropIndex("dbo.StockTbls", new[] { "TyreID" });
            AlterColumn("dbo.StockTbls", "TyreID", c => c.Int(nullable: true));
            CreateIndex("dbo.StockTbls", "TyreID");
            AddForeignKey("dbo.StockTbls", "TyreID", "dbo.TyresTbls", "TyreID", cascadeDelete: false);
        }
    }
}
