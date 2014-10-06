using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models
{
    public class TripRepo : ITripRepo, IDisposable
    {
        public TripRepo()
        {
            _context = new ApplicationDbContext();
        }

        private ApplicationDbContext _context;

        public ApplicationDbContext Context
        {
            get { return _context; } 
        }

        public IEnumerable<Trip> GetAll()
        {
            return _context.Trips.ToList();
        }

        public Trip Get(int id)
        {
            return _context.Trips.Where(x => x.Id == id).Include(t => t.Visits).Include(x => x.Routes).FirstOrDefault();
        }

        public Trip Add(Trip item)
        {
            _context.Trips.Add(item);
            _context.SaveChanges();
            return item;
        }

        public void Remove(int id)
        {
            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                _context.SaveChanges();
            }
                
        }

        public bool Update(Trip item)
        {
            Trip curr = _context.Trips.FirstOrDefault(x => x.Id == item.Id);
            if (curr == null)
                return false;

            curr.Name = item.Name;
            curr.Description = item.Description;
            curr.DateFrom = item.DateFrom;
            curr.DateTo = item.DateTo;
            curr.IsPrivate = item.IsPrivate;

            _context.SaveChanges();

            return true;
        }

        public Comment AddComment(int tripId, Comment comment)
        {
            Trip curr = _context.Trips.FirstOrDefault(x => x.Id == tripId);
            if (curr.Comments == null)
            {
                curr.Comments = new Collection<Comment>();
            }
            curr.Comments.Add(comment);
            _context.SaveChanges();

            return comment;
        }

        public void AddPhotos(int tripId, IEnumerable<Photo> photos)
        {
            Trip curr = _context.Trips.FirstOrDefault(x => x.Id == tripId);
            if (curr.Photos == null)
            {
                curr.Photos = new Collection<Photo>();
            }
            foreach (var tripPhoto in photos)
            {
                curr.Photos.Add(tripPhoto);
            }

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}