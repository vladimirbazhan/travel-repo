using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Visit : Entity, ITripEntity, IOrderedEntity
    {
        public string Description { get; set; }
        public double Cost { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public int Order { get; set; }

        public string GPlaceId { get; set; }

        [Required]
        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }

        public virtual ICollection<Comment> Comments { get; private set; }
        public virtual ICollection<Photo> Photos { get; private set; }
        public virtual ICollection<ApplicationUser> Members { get; private set; } 
    }
}