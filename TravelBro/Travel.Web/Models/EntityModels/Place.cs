using System.Collections.ObjectModel;
using System.Linq;

namespace WebApplication1.Models.EntityModels
{
    public class Place
    {
        public int Id { get; set; }

        public float Lat { get; set; }
        public float Long { get; set; }

        public string Name { get; set; }

        public virtual Collection<Route> From { get; set; }
        public virtual Collection<Route> To { get; set; }
        public Collection<Route> Routes {
            get { return new Collection<Route>(From.Concat(To).ToList()); }
        }
        public virtual Collection<Visit> Visits { get; private set; }
    }
}