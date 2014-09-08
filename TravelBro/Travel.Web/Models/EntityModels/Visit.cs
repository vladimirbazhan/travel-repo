using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.EntityModels
{
    public class Visit
    {
        public int Id { get; set; }

        public string Description { get; set; }
        public double Cost { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public int ActivityOrder { get; set; }

        public string GPlaceId { get; set; }

        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }

        public virtual Collection<Comment> Comments { get; private set; }
        public virtual Collection<Photo> Photos { get; private set; }
    }
}