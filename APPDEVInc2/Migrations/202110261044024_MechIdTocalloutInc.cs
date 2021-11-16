namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechIdTocalloutInc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceCalloutTbls", "MechanicID", c => c.Int(nullable: false));
            CreateIndex("dbo.InvoiceCalloutTbls", "MechanicID");
            AddForeignKey("dbo.InvoiceCalloutTbls", "MechanicID", "dbo.MechanicTbls", "MechanicID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceCalloutTbls", "MechanicID", "dbo.MechanicTbls");
            DropIndex("dbo.InvoiceCalloutTbls", new[] { "MechanicID" });
            DropColumn("dbo.InvoiceCalloutTbls", "MechanicID");
        }
    }
}
