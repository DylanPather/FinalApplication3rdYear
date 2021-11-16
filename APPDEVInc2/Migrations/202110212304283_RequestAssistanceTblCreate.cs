namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestAssistanceTblCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestAssistanceTbls",
                c => new
                    {
                        RequestID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        RequestAddress = c.String(),
                        TravelMode = c.String(),
                        TotalTime = c.String(),
                        TotalDistance = c.String(),
                        DescriptionOfProblem = c.String(),
                        RequestDate = c.DateTime(),
                        Status = c.String(),
                        QRCodeCheckIn = c.Binary(),
                    })
                .PrimaryKey(t => t.RequestID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RequestAssistanceTbls");
        }
    }
}
