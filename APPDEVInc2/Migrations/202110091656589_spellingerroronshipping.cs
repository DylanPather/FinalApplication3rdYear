namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spellingerroronshipping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PayFastShippings", "Is_Delivered", c => c.Boolean(nullable: false));
            AddColumn("dbo.PayFastShippings", "DeliveryNoteSigned", c => c.Boolean(nullable: false));
            DropColumn("dbo.PayFastShippings", "Is_Deliverd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PayFastShippings", "Is_Deliverd", c => c.Boolean(nullable: false));
            DropColumn("dbo.PayFastShippings", "DeliveryNoteSigned");
            DropColumn("dbo.PayFastShippings", "Is_Delivered");
        }
    }
}
