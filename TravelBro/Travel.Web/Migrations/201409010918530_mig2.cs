namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "Description", c => c.String());
            DropColumn("dbo.Routes", "Comment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routes", "Comment", c => c.String());
            DropColumn("dbo.Routes", "Description");
        }
    }
}
