namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statustotoolboxhistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToolBoxCheckInHistories", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToolBoxCheckInHistories", "Status");
        }
    }
}
