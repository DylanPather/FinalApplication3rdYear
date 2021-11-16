namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToolsCheck : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToolsTbls", "ToolBoxID", "dbo.ToolBoxTbls");
            DropIndex("dbo.ToolsTbls", new[] { "ToolBoxID" });
            DropColumn("dbo.ToolsTbls", "ToolBoxID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToolsTbls", "ToolBoxID", c => c.Int(nullable: false));
            CreateIndex("dbo.ToolsTbls", "ToolBoxID");
            AddForeignKey("dbo.ToolsTbls", "ToolBoxID", "dbo.ToolBoxTbls", "ToolBoxID", cascadeDelete: true);
        }
    }
}
