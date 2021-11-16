namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CallutTblChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalloutTbls", "DateEnRoute", c => c.DateTime());
            AddColumn("dbo.CalloutTbls", "DateArrived", c => c.DateTime());
            AddColumn("dbo.CalloutTbls", "DateComplete", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalloutTbls", "DateComplete");
            DropColumn("dbo.CalloutTbls", "DateArrived");
            DropColumn("dbo.CalloutTbls", "DateEnRoute");
        }
    }
}
