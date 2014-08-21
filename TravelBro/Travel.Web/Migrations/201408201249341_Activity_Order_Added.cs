namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Activity_Order_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visits", "ActivityOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Routes", "ActivityOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "ActivityOrder");
            DropColumn("dbo.Visits", "ActivityOrder");
        }
    }
}
