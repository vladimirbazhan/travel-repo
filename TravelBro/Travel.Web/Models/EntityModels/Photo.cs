using System;
using System.Collections.ObjectModel;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Photo
    {
        public int Id { get; set; }

        public DateTime Published { get; set; }
        public string ImagePath { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual Collection<Comment> Comments { get; private set; }
    }
}