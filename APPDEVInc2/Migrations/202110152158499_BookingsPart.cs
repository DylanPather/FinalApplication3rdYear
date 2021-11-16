namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingsPart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingTbls",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        VehicleID = c.Int(nullable: false),
                        BayID = c.Int(nullable: false),
                        DateBooked = c.DateTime(),
                        Status = c.String(),
                        BookingTbls_BookingID = c.Int(),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.BookingTbls", t => t.BookingTbls_BookingID)
                .Index(t => t.BookingTbls_BookingID);
            
            CreateTable(
                "dbo.CustomerVehicleTbls",
                c => new
                    {
                        CustomerVehicleID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerVehicleID)
                .ForeignKey("dbo.CustomerTbls", t => t.CustomerID, cascadeDelete: false)
                .ForeignKey("dbo.VehicleTbls", t => t.VehicleID, cascadeDelete: false)
                .Index(t => t.CustomerID)
                .Index(t => t.VehicleID);
            
            CreateTable(
                "dbo.VehicleTbls",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        VehicleRegNo = c.String(),
                        VehicleMake = c.String(),
                        VehicleModel = c.String(),
                        VehicleMileage = c.Int(nullable: false),
                        VehicleImage = c.Binary(),
                        Is_Active = c.Boolean(nullable: false),
                        Is_Delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleID);
            
            CreateTable(
                "dbo.ScheduleTbls",
                c => new
                    {
                        ScheduleID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        DateCheckIn = c.DateTime(),
                        DateCheckOut = c.DateTime(),
                        Status = c.String(),
                        CheckedIn = c.Boolean(nullable: false),
                        CheckedOut = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleID)
                .ForeignKey("dbo.BookingTbls", t => t.BookingID, cascadeDelete: false)
                .Index(t => t.BookingID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleTbls", "BookingID", "dbo.BookingTbls");
            DropForeignKey("dbo.CustomerVehicleTbls", "VehicleID", "dbo.VehicleTbls");
            DropForeignKey("dbo.CustomerVehicleTbls", "CustomerID", "dbo.CustomerTbls");
            DropForeignKey("dbo.BookingTbls", "BookingTbls_BookingID", "dbo.BookingTbls");
            DropIndex("dbo.ScheduleTbls", new[] { "BookingID" });
            DropIndex("dbo.CustomerVehicleTbls", new[] { "VehicleID" });
            DropIndex("dbo.CustomerVehicleTbls", new[] { "CustomerID" });
            DropIndex("dbo.BookingTbls", new[] { "BookingTbls_BookingID" });
            DropTable("dbo.ScheduleTbls");
            DropTable("dbo.VehicleTbls");
            DropTable("dbo.CustomerVehicleTbls");
            DropTable("dbo.BookingTbls");
        }
    }
}
