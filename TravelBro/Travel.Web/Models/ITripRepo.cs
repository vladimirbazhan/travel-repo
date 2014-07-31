using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models.EntityModels;

namespace WebApplication1.Models
{
    interface ITripRepo
    {
        IEnumerable<Trip> GetAll();
        Trip Get(int id);
        Trip Add(Trip item);
        void Remove(int id);
        bool Update(Trip item);
    }
}
