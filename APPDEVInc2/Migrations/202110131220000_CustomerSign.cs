namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerSign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdersTbls", "CustomerSignature", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdersTbls", "CustomerSignature");
        }
    }
}
