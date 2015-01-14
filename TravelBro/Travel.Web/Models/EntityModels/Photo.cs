using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Photo
    {
        public int Id { get; set; }

        public DateTime Published { get; set; }
        public string ImagePath { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Collection<Comment> Comments { get; private set; }

        public virtual Collection<Trip> PhotosToTrips { get; set; }
        public virtual Collection<Visit> PhotosToVisits { get; set; }
        public virtual Collection<Route> PhotosToRoutes { get; set; } 
    }
}