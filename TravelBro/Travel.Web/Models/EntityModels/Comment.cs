using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Comment : Entity
    {
        public int Id { get; set; }

        public DateTime Published { get; set; }
        public string Text { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Comment ReplyTo { get; set; }

        public virtual ICollection<Trip> CommentsToTrips { get; set; }
        public virtual ICollection<Visit> CommentsToVisits { get; set; }
        public virtual ICollection<Route> CommentsToRoutes { get; set; }
        public virtual ICollection<Photo> CommentsToPhotos { get; set; } 
    }
}