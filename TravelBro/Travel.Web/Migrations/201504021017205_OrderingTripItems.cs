namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderingTripItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Visits", "Order", c => c.Int(nullable: false));
            AlterColumn("dbo.Routes", "Start", c => c.DateTime());
            AlterColumn("dbo.Routes", "Finish", c => c.DateTime());
            AlterColumn("dbo.Visits", "Start", c => c.DateTime());
            AlterColumn("dbo.Visits", "Finish", c => c.DateTime());
            DropColumn("dbo.Routes", "ActivityOrder");
            DropColumn("dbo.Visits", "ActivityOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Visits", "ActivityOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Routes", "ActivityOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.Visits", "Finish", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Visits", "Start", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Routes", "Finish", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Routes", "Start", c => c.DateTime(nullable: false));
            DropColumn("dbo.Visits", "Order");
            DropColumn("dbo.Routes", "Order");
        }
    }
}
