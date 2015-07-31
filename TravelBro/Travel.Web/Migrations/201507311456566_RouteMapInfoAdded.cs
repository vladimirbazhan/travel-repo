namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RouteMapInfoAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "MapInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "MapInfo");
        }
    }
}
