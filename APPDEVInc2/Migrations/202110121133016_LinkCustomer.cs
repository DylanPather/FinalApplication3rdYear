namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkCustomer : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrdersTbls", "CustomerID");
            AddForeignKey("dbo.OrdersTbls", "CustomerID", "dbo.CustomerTbls", "CustomerID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdersTbls", "CustomerID", "dbo.CustomerTbls");
            DropIndex("dbo.OrdersTbls", new[] { "CustomerID" });
        }
    }
}
