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
            Order = r.Order;
            TransType = r.TransType;
            TripId = r.TripId;
            StartGPlaceId = r.StartGPlaceId;
            FinishGPlaceId = r.FinishGPlaceId;
            MapInfo = r.MapInfo;
            if (r.Comments != null)
            {
                Comments = (from comment in r.Comments select new CommentDTO(comment)).ToList();
            }
            if (r.Photos != null)
            {
                Photos = (from photo in r.Photos select new PhotoDTO(photo)).ToList();
            }
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public double Cost { get; set; }
        public int Order { get; set; }
        public TransType TransType { get; set; }
        public int TripId { get; set; }
        public string StartGPlaceId { get; set; }
        public string FinishGPlaceId { get; set; }
        public IEnumerable<CommentDTO> Comments;
        public IEnumerable<PhotoDTO> Photos;
        public string MapInfo { get; set; }
    }
}