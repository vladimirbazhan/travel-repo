using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.EntityModels
{
    public class PlaceDTO
    {
        public PlaceDTO(Place place)
        {
            Id = place.Id;
            Lat = place.Lat;
            Long = place.Long;
            Name = place.Name;
        }

        public int Id { get; set; }

        public float Lat { get; set; }
        public float Long { get; set; }

        public string Name { get; set; }
    }

}