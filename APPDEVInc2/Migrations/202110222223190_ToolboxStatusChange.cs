namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToolboxStatusChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ToolBoxTbls", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ToolBoxTbls", "Status", c => c.Boolean(nullable: false));
        }
    }
}
