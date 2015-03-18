namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Photos_Comments_AreFirstPlace : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhotosToComments", newName: "CommentsToPhotos");
            RenameTable(name: "dbo.RoutesToComments", newName: "CommentsToRoutes");
            RenameTable(name: "dbo.TripsToComments", newName: "CommentsToTrips");
            RenameTable(name: "dbo.VisitsToComments", newName: "CommentsToVisits");
            RenameTable(name: "dbo.TripsToPhotos", newName: "PhotosToTrips");
            RenameTable(name: "dbo.VisitsToPhotos", newName: "PhotosToVisits");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PhotosToVisits", newName: "VisitsToPhotos");
            RenameTable(name: "dbo.PhotosToTrips", newName: "TripsToPhotos");
            RenameTable(name: "dbo.CommentsToVisits", newName: "VisitsToComments");
            RenameTable(name: "dbo.CommentsToTrips", newName: "TripsToComments");
            RenameTable(name: "dbo.CommentsToRoutes", newName: "RoutesToComments");
            RenameTable(name: "dbo.CommentsToPhotos", newName: "PhotosToComments");
        }
    }
}
