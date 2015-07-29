namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapBoundsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "MapInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "MapInfo");
        }
    }
}
