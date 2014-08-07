namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "VisitId", "dbo.Visits");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Photos", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Visits", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Routes", "TripId", "dbo.Trips");
            DropIndex("dbo.Comments", new[] { "VisitId" });
            CreateTable(
                "dbo.PhotosToComments",
                c => new
                    {
                        PhotoId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PhotoId, t.CommentId })
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.PhotoId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.VisitsToComments",
                c => new
                    {
                        VisitId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VisitId, t.CommentId })
                .ForeignKey("dbo.Visits", t => t.VisitId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.VisitId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.RoutesToComments",
                c => new
                    {
                        RouteId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RouteId, t.CommentId })
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.RouteId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.TripsToComments",
                c => new
                    {
                        TripId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TripId, t.CommentId })
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.CommentId);
            
            AddColumn("dbo.Comments", "Place_Id", c => c.Int());
            AddColumn("dbo.Comments", "ReplyToCommentId", c => c.Int());
            CreateIndex("dbo.Comments", "Place_Id");
            CreateIndex("dbo.Comments", "ReplyToCommentId");
            AddForeignKey("dbo.Comments", "Place_Id", "dbo.Places", "Id");
            AddForeignKey("dbo.Comments", "ReplyToCommentId", "dbo.Comments", "Id");
            AddForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Photos", "AuthorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Visits", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Routes", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
            DropColumn("dbo.Comments", "VisitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "VisitId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Routes", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Visits", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Photos", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "ReplyToCommentId", "dbo.Comments");
            DropForeignKey("dbo.TripsToComments", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.TripsToComments", "TripId", "dbo.Trips");
            DropForeignKey("dbo.RoutesToComments", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.RoutesToComments", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.Comments", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.VisitsToComments", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.VisitsToComments", "VisitId", "dbo.Visits");
            DropForeignKey("dbo.PhotosToComments", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.PhotosToComments", "PhotoId", "dbo.Photos");
            DropIndex("dbo.TripsToComments", new[] { "CommentId" });
            DropIndex("dbo.TripsToComments", new[] { "TripId" });
            DropIndex("dbo.RoutesToComments", new[] { "CommentId" });
            DropIndex("dbo.RoutesToComments", new[] { "RouteId" });
            DropIndex("dbo.VisitsToComments", new[] { "CommentId" });
            DropIndex("dbo.VisitsToComments", new[] { "VisitId" });
            DropIndex("dbo.PhotosToComments", new[] { "CommentId" });
            DropIndex("dbo.PhotosToComments", new[] { "PhotoId" });
            DropIndex("dbo.Comments", new[] { "ReplyToCommentId" });
            DropIndex("dbo.Comments", new[] { "Place_Id" });
            DropColumn("dbo.Comments", "ReplyToCommentId");
            DropColumn("dbo.Comments", "Place_Id");
            DropTable("dbo.TripsToComments");
            DropTable("dbo.RoutesToComments");
            DropTable("dbo.VisitsToComments");
            DropTable("dbo.PhotosToComments");
            CreateIndex("dbo.Comments", "VisitId");
            AddForeignKey("dbo.Routes", "TripId", "dbo.Trips", "Id");
            AddForeignKey("dbo.Visits", "TripId", "dbo.Trips", "Id");
            AddForeignKey("dbo.Photos", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "VisitId", "dbo.Visits", "Id", cascadeDelete: true);
        }
    }
}
