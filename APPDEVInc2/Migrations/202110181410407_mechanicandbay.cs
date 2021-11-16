namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mechanicandbay : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BayTbls",
                c => new
                    {
                        BayID = c.Int(nullable: false, identity: true),
                        BayName = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.BayID);
            
            CreateTable(
                "dbo.MechanicTbls",
                c => new
                    {
                        MechanicID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ContactNo = c.String(),
                        VehicleMake = c.String(),
                        VehicleRegNo = c.String(),
                        VehicleImage = c.Binary(),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MechanicID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MechanicTbls");
            DropTable("dbo.BayTbls");
        }
    }
}
