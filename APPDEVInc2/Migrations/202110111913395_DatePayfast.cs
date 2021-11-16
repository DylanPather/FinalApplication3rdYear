namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatePayfast : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PayFastShippings", "DateTimeDelivered", c => c.DateTime());
            AlterColumn("dbo.PayFastShippings", "DateTimeEnRoute", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PayFastShippings", "DateTimeEnRoute", c => c.DateTime(nullable: true));
            AlterColumn("dbo.PayFastShippings", "DateTimeDelivered", c => c.DateTime(nullable: true));
        }
    }
}
