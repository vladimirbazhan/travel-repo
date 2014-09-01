using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.EntityModels
{
    public class Route
    {
        public int Id { get; set; }

        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public double Cost { get; set; }
        public int ActivityOrder { get; set; }

        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }

        public int StartPlaceId { get; set; }
        [ForeignKey("StartPlaceId")]
        public virtual Place StartPlace { get; set; }

        public int FinishPlaceId { get; set; }
        [ForeignKey("FinishPlaceId")]
        public virtual Place FinishPlace { get; set; }
        public virtual Collection<Comment> Comments { get; private set; }
    }
}