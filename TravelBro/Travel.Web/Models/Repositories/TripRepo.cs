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

        public TripRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
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
        }

        private void CleanUpPhotos()
        {
            parent.Repo<PhotoRepo>().ClearUnusedPhotos(x =>
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