using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Photo : Entity
    {
        public DateTime Published { get; set; }
        public string ImagePath { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Comment> Comments { get; private set; }

        public virtual ICollection<Trip> PhotosToTrips { get; set; }
        public virtual ICollection<Visit> PhotosToVisits { get; set; }
        public virtual ICollection<Route> PhotosToRoutes { get; set; } 
    }
}