namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class signatureformechanicontools : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToolsCheckOuts", "SignatureCheckOut", c => c.Binary());
            AddColumn("dbo.ToolsCheckOuts", "SignatureCheckIn", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToolsCheckOuts", "SignatureCheckIn");
            DropColumn("dbo.ToolsCheckOuts", "SignatureCheckOut");
        }
    }
}
