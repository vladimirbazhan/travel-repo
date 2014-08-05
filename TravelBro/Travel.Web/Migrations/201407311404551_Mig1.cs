namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "IsPrivate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "IsPrivate");
        }
    }
}
