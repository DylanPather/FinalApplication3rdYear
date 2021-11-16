namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingTbls", "CompBuilding", c => c.String());
            AddColumn("dbo.ShippingTbls", "City_Town", c => c.String());
            AddColumn("dbo.ShippingTbls", "Province", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShippingTbls", "Province");
            DropColumn("dbo.ShippingTbls", "City_Town");
            DropColumn("dbo.ShippingTbls", "CompBuilding");
        }
    }
}
