namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetailsTbls",
                c => new
                    {
                        OrderDetailID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        StockID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailID)
                .ForeignKey("dbo.OrdersTbls", t => t.OrderID, cascadeDelete: false)
                .ForeignKey("dbo.StockTbls", t => t.StockID, cascadeDelete: false)
                .Index(t => t.OrderID)
                .Index(t => t.StockID);
            
            CreateTable(
                "dbo.OrdersTbls",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetailsTbls", "StockID", "dbo.StockTbls");
            DropForeignKey("dbo.OrderDetailsTbls", "OrderID", "dbo.OrdersTbls");
            DropIndex("dbo.OrderDetailsTbls", new[] { "StockID" });
            DropIndex("dbo.OrderDetailsTbls", new[] { "OrderID" });
            DropTable("dbo.OrdersTbls");
            DropTable("dbo.OrderDetailsTbls");
        }
    }
}
