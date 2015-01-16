namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhotoComments", newName: "PhotosToComments");
            RenameTable(name: "dbo.RouteComments", newName: "RoutesToComments");
            RenameTable(name: "dbo.TripComments", newName: "TripsToComments");
            RenameTable(name: "dbo.VisitComments", newName: "VisitsToComments");
            RenameTable(name: "dbo.RouteApplicationUsers", newName: "RoutesToApplicationUsers");
            RenameTable(name: "dbo.VisitApplicationUsers", newName: "VisitsToApplicationUsers");
            RenameTable(name: "dbo.PhotoRoutes", newName: "PhotosToRoutes");
            RenameTable(name: "dbo.TripPhotoes", newName: "TripsToPhotos");
            RenameTable(name: "dbo.VisitPhotoes", newName: "VisitsToPhotos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.VisitsToPhotos", newName: "VisitPhotoes");
            RenameTable(name: "dbo.TripsToPhotos", newName: "TripPhotoes");
            RenameTable(name: "dbo.PhotosToRoutes", newName: "PhotoRoutes");
            RenameTable(name: "dbo.VisitsToApplicationUsers", newName: "VisitApplicationUsers");
            RenameTable(name: "dbo.RoutesToApplicationUsers", newName: "RouteApplicationUsers");
            RenameTable(name: "dbo.VisitsToComments", newName: "VisitComments");
            RenameTable(name: "dbo.TripsToComments", newName: "TripComments");
            RenameTable(name: "dbo.RoutesToComments", newName: "RouteComments");
            RenameTable(name: "dbo.PhotosToComments", newName: "PhotoComments");
        }
    }
}
