namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwitchToGooglePlaceId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.Routes", "FinishPlaceId", "dbo.Places");
            DropForeignKey("dbo.Routes", "StartPlaceId", "dbo.Places");
            DropForeignKey("dbo.Visits", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Trips", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "Place_Id" });
            DropIndex("dbo.Visits", new[] { "PlaceId" });
            DropIndex("dbo.Routes", new[] { "StartPlaceId" });
            DropIndex("dbo.Routes", new[] { "FinishPlaceId" });
            AddColumn("dbo.Visits", "GPlaceId", c => c.String());
            AddColumn("dbo.Routes", "StartGPlaceId", c => c.String());
            AddColumn("dbo.Routes", "FinishGPlaceId", c => c.String());
            AddForeignKey("dbo.Trips", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Comments", "Place_Id");
            DropColumn("dbo.Visits", "PlaceId");
            DropColumn("dbo.Routes", "StartPlaceId");
            DropColumn("dbo.Routes", "FinishPlaceId");
            DropTable("dbo.Places");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lat = c.Single(nullable: false),
                        Long = c.Single(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Routes", "FinishPlaceId", c => c.Int(nullable: false));
            AddColumn("dbo.Routes", "StartPlaceId", c => c.Int(nullable: false));
            AddColumn("dbo.Visits", "PlaceId", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "Place_Id", c => c.Int());
            DropForeignKey("dbo.Trips", "AuthorId", "dbo.AspNetUsers");
            DropColumn("dbo.Routes", "FinishGPlaceId");
            DropColumn("dbo.Routes", "StartGPlaceId");
            DropColumn("dbo.Visits", "GPlaceId");
            CreateIndex("dbo.Routes", "FinishPlaceId");
            CreateIndex("dbo.Routes", "StartPlaceId");
            CreateIndex("dbo.Visits", "PlaceId");
            CreateIndex("dbo.Comments", "Place_Id");
            AddForeignKey("dbo.Trips", "AuthorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Visits", "PlaceId", "dbo.Places", "Id");
            AddForeignKey("dbo.Routes", "StartPlaceId", "dbo.Places", "Id");
            AddForeignKey("dbo.Routes", "FinishPlaceId", "dbo.Places", "Id");
            AddForeignKey("dbo.Comments", "Place_Id", "dbo.Places", "Id");
        }
    }
}
