using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;
using WebApplication1.Models.Repositories;

namespace WebApplication1.Models
{
    public class TripRepo : Repository<Trip>
    {
        public string PhotoLocationPath { get; set; }

        public TripRepo(ApplicationDbContext context)
            : base(context)
        {
        }

        public override Trip Get(int id)
        {
            return FetchAll().Where(x => x.Id == id).Include(t => t.Visits).Include(x => x.Routes).FirstOrDefault();
        }

        public override void Delete(int id)
        {
            base.Delete(id);
            CleanUpPhotos();
        }

        public Comment AddComment(int tripId, Comment comment)
        {
            Trip curr = Get(tripId);
            if (curr.Comments == null)
            {
                curr.Comments = new Collection<Comment>();
            }
            curr.Comments.Add(comment);
            context.SaveChanges();

            return comment;
        }

        public void AddPhotos(int tripId, IEnumerable<Photo> photos)
        {
            Trip curr = Get(tripId);
            if (curr.Photos == null)
            {
                curr.Photos = new Collection<Photo>();
            }
            foreach (Photo tripPhoto in photos)
            {
                curr.Photos.Add(tripPhoto);
            }

            context.SaveChanges();
        }

        private void CleanUpPhotos()
        {
            context.ClearUnusedPhotos(x =>
            {
                try
                {
                    File.Delete(PhotoLocationPath + x.ImagePath);
                }
                catch (Exception)
                {
                }
            });
        }
    }
}