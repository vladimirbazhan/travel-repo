using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.EntityModels
{
    public class VisitDTO
    {
        public VisitDTO(Visit v)
        {
            Id = v.Id;
            Description = v.Description;
            Cost = v.Cost;
            Start = v.Start;
            Finish = v.Finish;
            Order = v.Order;
            GPlaceId = v.GPlaceId;
            TripId = v.TripId;
        }

        public int Id { get; set; }

        public string Description { get; set; }
        public double Cost { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public int Order { get; set; }

        public string GPlaceId { get; set; }
        public int TripId { get; set; }
    }
}