namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class historycommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HistoryTbls",
                c => new
                    {
                        HistID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        ShippingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HistID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HistoryTbls");
        }
    }
}
