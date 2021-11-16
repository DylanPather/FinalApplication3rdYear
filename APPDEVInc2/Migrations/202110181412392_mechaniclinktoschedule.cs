namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mechaniclinktoschedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScheduleTbls", "MechanicID", c => c.Int(nullable: false));
            CreateIndex("dbo.ScheduleTbls", "MechanicID");
            AddForeignKey("dbo.ScheduleTbls", "MechanicID", "dbo.MechanicTbls", "MechanicID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleTbls", "MechanicID", "dbo.MechanicTbls");
            DropIndex("dbo.ScheduleTbls", new[] { "MechanicID" });
            DropColumn("dbo.ScheduleTbls", "MechanicID");
        }
    }
}
