namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotosToX_Relations_Created : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "VisitId", "dbo.Visits");
            DropIndex("dbo.Photos", new[] { "VisitId" });
            CreateTable(
                "dbo.PhotosToRoutes",
                c => new
                    {
                        RouteId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RouteId, t.PhotoId })
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.RouteId)
                .Index(t => t.PhotoId);
            
            CreateTable(
                "dbo.PhotosToTrips",
                c => new
                    {
                        TripId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TripId, t.PhotoId })
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.PhotoId);
            
            CreateTable(
                "dbo.PhotosToVisits",
                c => new
                    {
                        VisitId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VisitId, t.PhotoId })
                .ForeignKey("dbo.Visits", t => t.VisitId, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.VisitId)
                .Index(t => t.PhotoId);
            
            DropColumn("dbo.Photos", "VisitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "VisitId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PhotosToVisits", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.PhotosToVisits", "VisitId", "dbo.Visits");
            DropForeignKey("dbo.PhotosToTrips", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.PhotosToTrips", "TripId", "dbo.Trips");
            DropForeignKey("dbo.PhotosToRoutes", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.PhotosToRoutes", "RouteId", "dbo.Routes");
            DropIndex("dbo.PhotosToVisits", new[] { "PhotoId" });
            DropIndex("dbo.PhotosToVisits", new[] { "VisitId" });
            DropIndex("dbo.PhotosToTrips", new[] { "PhotoId" });
            DropIndex("dbo.PhotosToTrips", new[] { "TripId" });
            DropIndex("dbo.PhotosToRoutes", new[] { "PhotoId" });
            DropIndex("dbo.PhotosToRoutes", new[] { "RouteId" });
            DropTable("dbo.PhotosToVisits");
            DropTable("dbo.PhotosToTrips");
            DropTable("dbo.PhotosToRoutes");
            CreateIndex("dbo.Photos", "VisitId");
            AddForeignKey("dbo.Photos", "VisitId", "dbo.Visits", "Id", cascadeDelete: true);
        }
    }
}
