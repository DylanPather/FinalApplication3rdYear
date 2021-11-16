namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingRelationChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BookingTbls", "BookingTbls_BookingID", "dbo.BookingTbls");
            DropIndex("dbo.BookingTbls", new[] { "BookingTbls_BookingID" });
            CreateIndex("dbo.BookingTbls", "VehicleID");
            AddForeignKey("dbo.BookingTbls", "VehicleID", "dbo.VehicleTbls", "VehicleID", cascadeDelete: false);
            DropColumn("dbo.BookingTbls", "BookingTbls_BookingID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookingTbls", "BookingTbls_BookingID", c => c.Int());
            DropForeignKey("dbo.BookingTbls", "VehicleID", "dbo.VehicleTbls");
            DropIndex("dbo.BookingTbls", new[] { "VehicleID" });
            CreateIndex("dbo.BookingTbls", "BookingTbls_BookingID");
            AddForeignKey("dbo.BookingTbls", "BookingTbls_BookingID", "dbo.BookingTbls", "BookingID");
        }
    }
}
