namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstCommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CID = c.Int(nullable: false, identity: true),
                        CartID = c.String(),
                        StockID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CID)
                .ForeignKey("dbo.StockTbls", t => t.StockID, cascadeDelete: false)
                .Index(t => t.StockID);
            
            CreateTable(
                "dbo.StockTbls",
                c => new
                    {
                        StockID = c.Int(nullable: false, identity: true),
                        ServiceID = c.Int(nullable: false),
                        TyreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockID)
                .ForeignKey("dbo.ServiceTbls", t => t.ServiceID, cascadeDelete: false)
                .ForeignKey("dbo.TyresTbls", t => t.TyreID, cascadeDelete: false)
                .Index(t => t.ServiceID)
                .Index(t => t.TyreID);
            
            CreateTable(
                "dbo.ServiceTbls",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Image = c.Binary(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ServiceID);
            
            CreateTable(
                "dbo.TyresTbls",
                c => new
                    {
                        TyreID = c.Int(nullable: false, identity: true),
                        TyreSize = c.String(),
                        TyreName = c.String(),
                        TyreBrand = c.String(),
                        SellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        SLA = c.String(),
                        Image = c.Binary(),
                        IsFeatured = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TyreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "StockID", "dbo.StockTbls");
            DropForeignKey("dbo.StockTbls", "TyreID", "dbo.TyresTbls");
            DropForeignKey("dbo.StockTbls", "ServiceID", "dbo.ServiceTbls");
            DropIndex("dbo.StockTbls", new[] { "TyreID" });
            DropIndex("dbo.StockTbls", new[] { "ServiceID" });
            DropIndex("dbo.Carts", new[] { "StockID" });
            DropTable("dbo.TyresTbls");
            DropTable("dbo.ServiceTbls");
            DropTable("dbo.StockTbls");
            DropTable("dbo.Carts");
        }
    }
}
