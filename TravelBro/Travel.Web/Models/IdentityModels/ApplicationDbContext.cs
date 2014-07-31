using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.Author)
                .WithMany(a => a.Trips)
                .Map(m => m.MapKey("AuthorId"))
                .WillCascadeOnDelete(false);

            #region Visit
            modelBuilder.Entity<Visit>()
                .HasRequired(v => v.Trip)
                .WithMany(t => t.Visits)
                .Map(m => m.MapKey("TripId"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Visit>()
                .HasRequired(v => v.Place)
                .WithMany(p => p.Visits)
                .Map(m => m.MapKey("PlaceId"))
                .WillCascadeOnDelete(false);
            #endregion

            #region Route
            modelBuilder.Entity<Route>()
                .HasRequired(v => v.Trip)
                .WithMany(t => t.Routes)
                .Map(m => m.MapKey("TripId"))
                .WillCascadeOnDelete(false);

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
            #endregion

            #region Comments & Photos
            modelBuilder.Entity<Comment>()
                    .HasRequired(c => c.Author)
                    .WithMany(a => a.Comments)
                    .Map(m => m.MapKey("AuthorId"));
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Visit)
                .WithMany(v => v.Comments)
                .Map(m => m.MapKey("VisitId"));

            modelBuilder.Entity<Photo>()
                .HasRequired(p => p.Author)
                .WithMany(a => a.Photos)
                .Map(m => m.MapKey("AuthorId"));
            modelBuilder.Entity<Photo>()
                .HasRequired(photo => photo.Visit)
                .WithMany(v => v.Photos)
                .Map(m => m.MapKey("VisitId")); 
            #endregion

        }
    }
}