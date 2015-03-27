using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebApplication1.Models.EntityModels
{
    public enum TransTypeEnum : int
    {
        NotSpecified = 0,
        Plane = 1,
        Car = 2,
        Bus = 3,
        Train = 4,
        Ship = 5,
        Motorbike = 6,
        Bike = 7,
        Pedestrian = 8,
    }

    public class TransType : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }

        public string Name { get; set; }
    }
}