namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletecustomerorder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrdersTbls", "Customertbl_CustomerID", "dbo.CustomerTbls");
            DropIndex("dbo.OrdersTbls", new[] { "Customertbl_CustomerID" });
            DropColumn("dbo.OrdersTbls", "Customertbl_CustomerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdersTbls", "Customertbl_CustomerID", c => c.Int());
            CreateIndex("dbo.OrdersTbls", "Customertbl_CustomerID");
            AddForeignKey("dbo.OrdersTbls", "Customertbl_CustomerID", "dbo.CustomerTbls", "CustomerID");
        }
    }
}
