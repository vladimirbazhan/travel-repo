using System;
using System.Collections.ObjectModel;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Comment
    {
        public int Id { get; set; }

        public DateTime Published { get; set; }
        public string Text { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Comment ReplyTo { get; set; }

        public virtual Collection<Trip> CommentsToTrips { get; set; }
        public virtual Collection<Visit> CommentsToVisits { get; set; }
        public virtual Collection<Route> CommentsToRoutes { get; set; }
        public virtual Collection<Photo> CommentsToPhotos { get; set; } 
    }
}