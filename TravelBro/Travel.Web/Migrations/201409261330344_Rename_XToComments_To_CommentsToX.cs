namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_XToComments_To_CommentsToX : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhotosToComments", newName: "CommentsToPhotos");
            RenameTable(name: "dbo.VisitsToComments", newName: "CommentsToVisits");
            RenameTable(name: "dbo.TripsToComments", newName: "CommentsToTrips");
            RenameTable(name: "dbo.RoutesToComments", newName: "CommentsToRoutes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CommentsToRoutes", newName: "RoutesToComments");
            RenameTable(name: "dbo.CommentsToTrips", newName: "TripsToComments");
            RenameTable(name: "dbo.CommentsToVisits", newName: "VisitsToComments");
            RenameTable(name: "dbo.CommentsToPhotos", newName: "PhotosToComments");
        }
    }
}
