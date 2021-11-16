namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerTblCommit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomerTbls", "PointsBalance");
            DropColumn("dbo.CustomerTbls", "LoyaltyStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerTbls", "LoyaltyStatus", c => c.String());
            AddColumn("dbo.CustomerTbls", "PointsBalance", c => c.Int(nullable: false));
        }
    }
}
