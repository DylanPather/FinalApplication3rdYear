namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriverNameinDrivertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "FullName");
        }
    }
}
