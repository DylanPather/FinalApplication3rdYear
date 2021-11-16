namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToolBoxToolTableLink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToolBoxToolDetails",
                c => new
                    {
                        ToolBoxDetailID = c.Int(nullable: false, identity: true),
                        ToolBoxID = c.Int(nullable: false),
                        ToolID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ToolBoxDetailID)
                .ForeignKey("dbo.ToolBoxTbls", t => t.ToolBoxID, cascadeDelete: false)
                .ForeignKey("dbo.ToolsTbls", t => t.ToolID, cascadeDelete: false)
                .Index(t => t.ToolBoxID)
                .Index(t => t.ToolID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToolBoxToolDetails", "ToolID", "dbo.ToolsTbls");
            DropForeignKey("dbo.ToolBoxToolDetails", "ToolBoxID", "dbo.ToolBoxTbls");
            DropIndex("dbo.ToolBoxToolDetails", new[] { "ToolID" });
            DropIndex("dbo.ToolBoxToolDetails", new[] { "ToolBoxID" });
            DropTable("dbo.ToolBoxToolDetails");
        }
    }
}
