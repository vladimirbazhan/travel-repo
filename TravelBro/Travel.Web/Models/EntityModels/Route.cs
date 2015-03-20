using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Route : Entity
    {
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public double Cost { get; set; }
        public int ActivityOrder { get; set; }

        [Required]
        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }

        public string StartGPlaceId { get; set; }
        public string FinishGPlaceId { get; set; }

        public virtual ICollection<Comment> Comments { get; private set; }
        public virtual ICollection<Photo> Photos { get; private set; }
        public virtual ICollection<ApplicationUser> Members { get; private set; } 
    }
}