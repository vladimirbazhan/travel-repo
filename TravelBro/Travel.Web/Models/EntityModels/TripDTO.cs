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
            AuthorEmail = trip.Author.Email;
            Name = trip.Name;
            Description = trip.Description;
            DateFrom = trip.DateFrom;
            DateTo = trip.DateTo;
            IsPrivate = trip.IsPrivate;
            MapInfo = trip.MapInfo;
            if (trip.Visits != null)
            {
                Visits = (from visit in trip.Visits select new VisitDTO(visit)).ToList();
            }
            if (trip.Routes != null)
            {
                Routes = (from route in trip.Routes select new RouteDTO(route)).ToList();
            }
            if (trip.Comments != null)
            {
                Comments = (from comment in trip.Comments select new CommentDTO(comment)).ToList();
            }
            if (trip.Photos != null)
            {
                Photos = (from photo in trip.Photos select new PhotoDTO(photo)).ToList();
            }
        }

        public int Id { get; set; }
        public string AuthorEmail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MapInfo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool IsPrivate { get; set; }
        public IEnumerable<VisitDTO> Visits;
        public IEnumerable<RouteDTO> Routes;
        public IEnumerable<CommentDTO> Comments;
        public IEnumerable<PhotoDTO> Photos;
    }
}