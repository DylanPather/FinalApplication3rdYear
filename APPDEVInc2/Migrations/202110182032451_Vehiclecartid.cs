namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vehiclecartid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReportCarts", "VehicleID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReportCarts", "VehicleID", c => c.Int(nullable: false));
        }
    }
}
