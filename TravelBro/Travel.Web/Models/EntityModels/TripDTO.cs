using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.EntityModels
{
    public class TripDTO
    {
        public TripDTO(Trip trip)
        {
            Id = trip.Id;
            Name = trip.Name;
            Description = trip.Description;
            DateFrom = trip.DateFrom;
            DateTo = trip.DateTo;
            IsPrivate = trip.IsPrivate;
            if (trip.Visits != null)
            {
                Visits = from visit in trip.Visits select new VisitDTO(visit);
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool IsPrivate { get; set; }
        public IEnumerable<VisitDTO> Visits;
    }
}