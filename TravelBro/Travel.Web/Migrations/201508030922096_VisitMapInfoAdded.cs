namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VisitMapInfoAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visits", "MapInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Visits", "MapInfo");
        }
    }
}
