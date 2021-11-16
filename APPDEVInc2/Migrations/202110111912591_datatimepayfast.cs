namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatimepayfast : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PayFastShippings", "DateTimeDelivered", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PayFastShippings", "DateTimeEnRoute", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PayFastShippings", "DateTimeEnRoute", c => c.DateTime());
            AlterColumn("dbo.PayFastShippings", "DateTimeDelivered", c => c.DateTime());
        }
    }
}
