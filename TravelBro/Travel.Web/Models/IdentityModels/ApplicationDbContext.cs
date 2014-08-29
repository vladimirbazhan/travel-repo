using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Migrations;
using WebApplication1.Models.EntityModels;

namespace WebApplication1.Models.IdentityModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        private static ApplicationDbContext _instance;

        public static ApplicationDbContext GetInstance()
        {
            if (_instance == null)
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());

            return _instance ?? (_instance = new ApplicationDbContext());
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Place>()
                .Ignore(p => p.Routes);

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.Author)
                .WithMany(a => a.Trips)
                .Map(m => m.MapKey("AuthorId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("TripsToComments");
                    m.MapLeftKey("TripId");
                    m.MapRightKey("CommentId");
                });

            #region Visit

            modelBuilder.Entity<Visit>()
                .HasRequired(v => v.Trip)
                .WithMany(t => t.Visits)
                .Map(m => m.MapKey("TripId"));

            modelBuilder.Entity<Visit>()
                .HasRequired(v => v.Place)
                .WithMany(p => p.Visits)
                .Map(m => m.MapKey("PlaceId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Visit>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("VisitsToComments");
                    m.MapLeftKey("VisitId");
                    m.MapRightKey("CommentId");
                });
            #endregion

            #region Route

            modelBuilder.Entity<Route>()
                .HasRequired(v => v.Trip)
                .WithMany(t => t.Routes)
                .Map(m => m.MapKey("TripId"));

            modelBuilder.Entity<Route>()
                .HasRequired(v => v.StartPlace)
                .WithMany(p => p.From)
                .Map(m => m.MapKey("StartPlaceId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasRequired(v => v.FinishPlace)
                .WithMany(p => p.To)
                .Map(m => m.MapKey("FinishPlaceId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("RoutesToComments");
                    m.MapLeftKey("RouteId");
                    m.MapRightKey("CommentId");
                });
            #endregion

            #region Comments & Photos
            modelBuilder.Entity<Comment>()
                    .HasRequired(c => c.Author)
                    .WithMany(a => a.Comments)
                    .Map(m => m.MapKey("AuthorId"))
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .HasOptional(c => c.ReplyTo)
                .WithOptionalDependent()
                .Map(m => m.MapKey("ReplyToCommentId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photo>()
                .HasRequired(p => p.Author)
                .WithMany(a => a.Photos)
                .Map(m => m.MapKey("AuthorId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photo>()
                .HasRequired(photo => photo.Visit)
                .WithMany(v => v.Photos)
                .Map(m => m.MapKey("VisitId"));

            modelBuilder.Entity<Photo>()
                .HasMany(t => t.Comments)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("PhotosToComments");
                    m.MapLeftKey("PhotoId");
                    m.MapRightKey("CommentId");
                });
            #endregion

        }
    }
}