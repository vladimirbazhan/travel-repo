using System;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Photo
    {
        public int Id { get; set; }

        public DateTime Published { get; set; }
        public string ImagePath { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Visit Visit { get; set; }
    }
}