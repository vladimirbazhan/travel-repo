namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoIsMainAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "IsMain", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "IsMain");
        }
    }
}
