namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerTbl1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerTbls", "CustomerTbl_CustomerID", "dbo.CustomerTbls");
            DropIndex("dbo.CustomerTbls", new[] { "CustomerTbl_CustomerID" });
            DropColumn("dbo.CustomerTbls", "CustomerTbl_CustomerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerTbls", "CustomerTbl_CustomerID", c => c.Int());
            CreateIndex("dbo.CustomerTbls", "CustomerTbl_CustomerID");
            AddForeignKey("dbo.CustomerTbls", "CustomerTbl_CustomerID", "dbo.CustomerTbls", "CustomerID");
        }
    }
}
