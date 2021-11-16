namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayFastShipAdjust : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PayFastShippings", "IsPickedUp", c => c.Boolean(nullable: false));
            AddColumn("dbo.PayFastShippings", "DateTimePickedUp", c => c.DateTime());
            AddColumn("dbo.PayFastShippings", "DispatchSignImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PayFastShippings", "DispatchSignImage");
            DropColumn("dbo.PayFastShippings", "DateTimePickedUp");
            DropColumn("dbo.PayFastShippings", "IsPickedUp");
        }
    }
}
