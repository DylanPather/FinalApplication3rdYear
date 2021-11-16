namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicHasBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingTbls", "HasMechanic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingTbls", "HasMechanic");
        }
    }
}
