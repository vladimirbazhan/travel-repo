using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.EntityModels
{
    public class RouteDTO
    {
        public RouteDTO(Route r)
        {
            Id = r.Id;
            Description = r.Description;
            Start = r.Start;
            Finish = r.Finish;
            Cost = r.Cost;
            ActivityOrder = r.ActivityOrder;
            TripId = r.TripId;
            StartPlace = new PlaceDTO(r.StartPlace);
            FinishPlace = new PlaceDTO(r.FinishPlace);
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public double Cost { get; set; }
        public int ActivityOrder { get; set; }
        public int TripId { get; set; }
        public PlaceDTO StartPlace { get; set; }
        public PlaceDTO FinishPlace { get; set; }
    }
}