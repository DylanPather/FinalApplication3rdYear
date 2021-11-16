namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerTbls",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(),
                        FirstName = c.String(),
                        Surname = c.String(),
                        ContactNo = c.String(),
                        PointsBalance = c.Int(nullable: false),
                        LoyaltyStatus = c.String(),
                        UserID = c.String(),
                        CustomerTbl_CustomerID = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.CustomerTbls", t => t.CustomerTbl_CustomerID)
                .Index(t => t.CustomerTbl_CustomerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerTbls", "CustomerTbl_CustomerID", "dbo.CustomerTbls");
            DropIndex("dbo.CustomerTbls", new[] { "CustomerTbl_CustomerID" });
            DropTable("dbo.CustomerTbls");
        }
    }
}
