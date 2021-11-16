namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HistorytblOrderid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HistoryTbls", "OrderID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HistoryTbls", "OrderID");
        }
    }
}
