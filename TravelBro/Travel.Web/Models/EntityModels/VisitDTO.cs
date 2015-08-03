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
            MapInfo = v.MapInfo;
            if (v.Comments != null)
            {
                Comments = (from comment in v.Comments select new CommentDTO(comment)).ToList();
            }
            if (v.Photos != null)
            {
                Photos = (from photo in v.Photos select new PhotoDTO(photo)).ToList();
            }
        }

        public int Id { get; set; }

        public string Description { get; set; }
        public double Cost { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public int Order { get; set; }
        public string MapInfo { get; set; }

        public string GPlaceId { get; set; }
        public int TripId { get; set; }

        public IEnumerable<CommentDTO> Comments;
        public IEnumerable<PhotoDTO> Photos;
    }
}