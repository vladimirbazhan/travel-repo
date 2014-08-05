namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        VisitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Visits", t => t.VisitId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.VisitId);
            
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
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
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Published = c.DateTime(nullable: false),
                        ImagePath = c.String(),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        VisitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Visits", t => t.VisitId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.VisitId);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Cost = c.Double(nullable: false),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(nullable: false),
                        PlaceId = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.PlaceId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .Index(t => t.PlaceId)
                .Index(t => t.TripId);
            
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
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(nullable: false),
                        Cost = c.Double(nullable: false),
                        FinishPlaceId = c.Int(nullable: false),
                        StartPlaceId = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                        Place_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.FinishPlaceId)
                .ForeignKey("dbo.Places", t => t.StartPlaceId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.Places", t => t.Place_Id)
                .Index(t => t.FinishPlaceId)
                .Index(t => t.StartPlaceId)
                .Index(t => t.TripId)
                .Index(t => t.Place_Id);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Comments", "VisitId", "dbo.Visits");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Photos", "VisitId", "dbo.Visits");
            DropForeignKey("dbo.Visits", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Visits", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Routes", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.Routes", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Trips", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Routes", "StartPlaceId", "dbo.Places");
            DropForeignKey("dbo.Routes", "FinishPlaceId", "dbo.Places");
            DropForeignKey("dbo.Photos", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Trips", new[] { "AuthorId" });
            DropIndex("dbo.Routes", new[] { "Place_Id" });
            DropIndex("dbo.Routes", new[] { "TripId" });
            DropIndex("dbo.Routes", new[] { "StartPlaceId" });
            DropIndex("dbo.Routes", new[] { "FinishPlaceId" });
            DropIndex("dbo.Visits", new[] { "TripId" });
            DropIndex("dbo.Visits", new[] { "PlaceId" });
            DropIndex("dbo.Photos", new[] { "VisitId" });
            DropIndex("dbo.Photos", new[] { "AuthorId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "VisitId" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Trips");
            DropTable("dbo.Routes");
            DropTable("dbo.Places");
            DropTable("dbo.Visits");
            DropTable("dbo.Photos");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
        }
    }
}
