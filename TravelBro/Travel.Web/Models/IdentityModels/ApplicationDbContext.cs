﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Migrations;
using WebApplication1.Models.Conventions;
using WebApplication1.Models.EntityModels;
using WebGrease.Css.Extensions;

namespace WebApplication1.Models.IdentityModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // entity processing delegates
        public delegate void ProcessPhotoDelegate(Photo photo);

        private static bool _dbInitialized = false;

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Initialize();
        }

        private static void Initialize()
        {
            if (!_dbInitialized)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
                _dbInitialized = true;
            }
        }
        
        public static ApplicationDbContext CreateInstance()
        {
            Initialize();
            return new ApplicationDbContext();
        }
        
        public void ClearUnusedPhotos(ProcessPhotoDelegate processPhoto)
        {
            // TODO: performance issues could arise because we fetch too much data from DB, remember about it
            // all photos
            var photoes = Photos.Include(x => x.PhotosToRoutes).Include(x => x.PhotosToTrips).Include(x => x.PhotosToVisits).ToList();
            // unused photos
            var unusedPhotos = photoes.Where(x => x.PhotosToRoutes.Count == 0 && x.PhotosToTrips.Count == 0 && x.PhotosToVisits.Count == 0);
            unusedPhotos.ForEach(x => processPhoto(x));
            Photos.RemoveRange(unusedPhotos);
            SaveChanges();
        }
        
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Add(new ManyToManyTableNameConvention());
        }
    }
}