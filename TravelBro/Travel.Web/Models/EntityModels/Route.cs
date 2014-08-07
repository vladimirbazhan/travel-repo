using System;
using System.Collections.ObjectModel;

namespace WebApplication1.Models.EntityModels
{
    public class Route
    {
        public int Id { get; set; }

        public string Comment { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public double Cost { get; set; }

        public virtual Trip Trip { get; set; }

        public virtual Place StartPlace { get; set; }

        public virtual Place FinishPlace { get; set; }
        public virtual Collection<Comment> Comments { get; private set; }
    }
}