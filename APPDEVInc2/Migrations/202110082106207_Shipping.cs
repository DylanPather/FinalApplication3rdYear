namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shipping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        VehicleMake = c.String(),
                        VehicleRegNo = c.String(),
                        VehicleImage = c.Binary(),
                        IsAvailable = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.DriverID);
            
            CreateTable(
                "dbo.PayFastShippings",
                c => new
                    {
                        PayFastShippingID = c.Int(nullable: false, identity: true),
                        DriverID = c.Int(nullable: false),
                        ShippingID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Status = c.String(),
                        Is_Deliverd = c.Boolean(nullable: false),
                        DateTimeDelivered = c.DateTime(nullable: false),
                        DateTimeEnRoute = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PayFastShippingID)
                .ForeignKey("dbo.Drivers", t => t.DriverID, cascadeDelete: false)
                .ForeignKey("dbo.OrdersTbls", t => t.OrderID, cascadeDelete: false)
                .ForeignKey("dbo.ShippingTbls", t => t.ShippingID, cascadeDelete: false)
                .Index(t => t.DriverID)
                .Index(t => t.ShippingID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.ShippingTbls",
                c => new
                    {
                        ShippingID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        StreetAddress = c.String(),
                        Suburb = c.String(),
                        PostalCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShippingID);
            
            AddColumn("dbo.OrdersTbls", "QRCodeImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayFastShippings", "ShippingID", "dbo.ShippingTbls");
            DropForeignKey("dbo.PayFastShippings", "OrderID", "dbo.OrdersTbls");
            DropForeignKey("dbo.PayFastShippings", "DriverID", "dbo.Drivers");
            DropIndex("dbo.PayFastShippings", new[] { "OrderID" });
            DropIndex("dbo.PayFastShippings", new[] { "ShippingID" });
            DropIndex("dbo.PayFastShippings", new[] { "DriverID" });
            DropColumn("dbo.OrdersTbls", "QRCodeImage");
            DropTable("dbo.ShippingTbls");
            DropTable("dbo.PayFastShippings");
            DropTable("dbo.Drivers");
        }
    }
}
