namespace APPDEVInc2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalloutServicesList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalloutServices",
                c => new
                    {
                        CalloutServiceID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CalloutServiceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CalloutServices");
        }
    }
}
