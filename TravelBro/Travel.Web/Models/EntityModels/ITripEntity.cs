using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.EntityModels
{
    public interface ITripEntity : IEntity
    {
        int TripId { get; set; }
        Trip Trip { get; set; }
    }
}