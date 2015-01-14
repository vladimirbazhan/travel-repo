using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool IsPrivate { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Collection<Route> Routes { get; private set; }
        public virtual Collection<Visit> Visits { get; private set; }

        public virtual Collection<Comment> Comments { get; set; }
        public virtual Collection<Photo> Photos { get; set; }
        public virtual Collection<ApplicationUser> Members { get; set; } 
    }
}