namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Published = c.DateTime(nullable: false),
                        Text = c.String(),
                        Author_Id = c.String(maxLength: 128),
                        ReplyTo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Comments", t => t.ReplyTo_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.ReplyTo_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VisibleName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Trip_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.Trip_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Trip_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(nullable: false),
                        Cost = c.Double(nullable: false),
                        ActivityOrder = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                        StartGPlaceId = c.String(),
                        FinishGPlaceId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Published = c.DateTime(nullable: false),
                        ImagePath = c.String(),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Cost = c.Double(nullable: false),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(nullable: false),
                        ActivityOrder = c.Int(nullable: false),
                        GPlaceId = c.String(),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.RouteComments",
                c => new
                    {
                        Route_Id = c.Int(nullable: false),
                        Comment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Route_Id, t.Comment_Id })
                .ForeignKey("dbo.Routes", t => t.Route_Id, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comment_Id, cascadeDelete: true)
                .Index(t => t.Route_Id)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.RouteApplicationUsers",
                c => new
                    {
                        Route_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Route_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Routes", t => t.Route_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Route_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.PhotoComments",
                c => new
                    {
                        Photo_Id = c.Int(nullable: false),
                        Comment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Photo_Id, t.Comment_Id })
                .ForeignKey("dbo.Photos", t => t.Photo_Id, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comment_Id, cascadeDelete: true)
                .Index(t => t.Photo_Id)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.PhotoRoutes",
                c => new
                    {
                        Photo_Id = c.Int(nullable: false),
                        Route_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Photo_Id, t.Route_Id })
                .ForeignKey("dbo.Photos", t => t.Photo_Id, cascadeDelete: true)
                .ForeignKey("dbo.Routes", t => t.Route_Id, cascadeDelete: true)
                .Index(t => t.Photo_Id)
                .Index(t => t.Route_Id);
            
            CreateTable(
                "dbo.TripComments",
                c => new
                    {
                        Trip_Id = c.Int(nullable: false),
                        Comment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trip_Id, t.Comment_Id })
                .ForeignKey("dbo.Trips", t => t.Trip_Id, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comment_Id, cascadeDelete: true)
                .Index(t => t.Trip_Id)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.TripPhotoes",
                c => new
                    {
                        Trip_Id = c.Int(nullable: false),
                        Photo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trip_Id, t.Photo_Id })
                .ForeignKey("dbo.Trips", t => t.Trip_Id, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.Photo_Id, cascadeDelete: true)
                .Index(t => t.Trip_Id)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.VisitComments",
                c => new
                    {
                        Visit_Id = c.Int(nullable: false),
                        Comment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Visit_Id, t.Comment_Id })
                .ForeignKey("dbo.Visits", t => t.Visit_Id, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comment_Id, cascadeDelete: true)
                .Index(t => t.Visit_Id)
                .Index(t => t.Comment_Id);
            
            CreateTable(
                "dbo.VisitApplicationUsers",
                c => new
                    {
                        Visit_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Visit_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Visits", t => t.Visit_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Visit_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.VisitPhotoes",
                c => new
                    {
                        Visit_Id = c.Int(nullable: false),
                        Photo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Visit_Id, t.Photo_Id })
                .ForeignKey("dbo.Visits", t => t.Visit_Id, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.Photo_Id, cascadeDelete: true)
                .Index(t => t.Visit_Id)
                .Index(t => t.Photo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Comments", "ReplyTo_Id", "dbo.Comments");
            DropForeignKey("dbo.Comments", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trips", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Visits", "TripId", "dbo.Trips");
            DropForeignKey("dbo.VisitPhotoes", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.VisitPhotoes", "Visit_Id", "dbo.Visits");
            DropForeignKey("dbo.VisitApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.VisitApplicationUsers", "Visit_Id", "dbo.Visits");
            DropForeignKey("dbo.VisitComments", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.VisitComments", "Visit_Id", "dbo.Visits");
            DropForeignKey("dbo.Routes", "TripId", "dbo.Trips");
            DropForeignKey("dbo.TripPhotoes", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.TripPhotoes", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.AspNetUsers", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.TripComments", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.TripComments", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.Trips", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PhotoRoutes", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.PhotoRoutes", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.PhotoComments", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.PhotoComments", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.Photos", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RouteApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RouteApplicationUsers", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.RouteComments", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.RouteComments", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.VisitPhotoes", new[] { "Photo_Id" });
            DropIndex("dbo.VisitPhotoes", new[] { "Visit_Id" });
            DropIndex("dbo.VisitApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.VisitApplicationUsers", new[] { "Visit_Id" });
            DropIndex("dbo.VisitComments", new[] { "Comment_Id" });
            DropIndex("dbo.VisitComments", new[] { "Visit_Id" });
            DropIndex("dbo.TripPhotoes", new[] { "Photo_Id" });
            DropIndex("dbo.TripPhotoes", new[] { "Trip_Id" });
            DropIndex("dbo.TripComments", new[] { "Comment_Id" });
            DropIndex("dbo.TripComments", new[] { "Trip_Id" });
            DropIndex("dbo.PhotoRoutes", new[] { "Route_Id" });
            DropIndex("dbo.PhotoRoutes", new[] { "Photo_Id" });
            DropIndex("dbo.PhotoComments", new[] { "Comment_Id" });
            DropIndex("dbo.PhotoComments", new[] { "Photo_Id" });
            DropIndex("dbo.RouteApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.RouteApplicationUsers", new[] { "Route_Id" });
            DropIndex("dbo.RouteComments", new[] { "Comment_Id" });
            DropIndex("dbo.RouteComments", new[] { "Route_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Visits", new[] { "TripId" });
            DropIndex("dbo.Trips", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Trips", new[] { "Author_Id" });
            DropIndex("dbo.Photos", new[] { "Author_Id" });
            DropIndex("dbo.Routes", new[] { "TripId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Trip_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "ReplyTo_Id" });
            DropIndex("dbo.Comments", new[] { "Author_Id" });
            DropTable("dbo.VisitPhotoes");
            DropTable("dbo.VisitApplicationUsers");
            DropTable("dbo.VisitComments");
            DropTable("dbo.TripPhotoes");
            DropTable("dbo.TripComments");
            DropTable("dbo.PhotoRoutes");
            DropTable("dbo.PhotoComments");
            DropTable("dbo.RouteApplicationUsers");
            DropTable("dbo.RouteComments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Visits");
            DropTable("dbo.Trips");
            DropTable("dbo.Photos");
            DropTable("dbo.Routes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
        }
    }
}
