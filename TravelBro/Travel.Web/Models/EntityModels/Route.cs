using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Route : Entity, ITripEntity, IOrderedEntity
    {
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public double Cost { get; set; }
        public int Order { get; set; }
        public string MapInfo { get; set; }

        public int TransTypeId { get; set; }
        private TransType transType;
        [ForeignKey("TransTypeId")]
        public virtual TransType TransType {
            get { return transType; }
            set
            {
                transType = value;
                TransTypeId = value.Id;
            } 
        }

        [Required]
        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }

        public string StartGPlaceId { get; set; }
        public string FinishGPlaceId { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }

        public void Merge(Route other)
        {
            Description = other.Description;
            Cost = other.Cost;
            Start = other.Start;
            Finish = other.Finish;
            Order = other.Order;
            TransTypeId = other.TransTypeId;
            StartGPlaceId = other.StartGPlaceId;
            FinishGPlaceId = other.FinishGPlaceId;
            MapInfo = other.MapInfo;
        }
    }
}