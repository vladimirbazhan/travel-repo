using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TripRepo : ITripRepo
    {
        private List<Trip> _trips = new List<Trip>() 
        {
            new Trip() { Id = 1, Name = "Poland", Description = "My first visit to Poland" },
            new Trip() { Id = 2, Name = "Germany", Description = "My memories about Berlin" },
            new Trip() { Id = 3, Name = "Turkish", Description = "Why do I love Istanbul" }
        };

        private int _nextId = 4;

        public IEnumerable<Trip> GetAll()
        {
            return _trips;
        }

        public Trip Get(int id)
        {
            return _trips.FirstOrDefault(x => x.Id == id);
        }

        public Trip Add(Trip item)
        {
            item.Id = _nextId++;
            _trips.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            _trips.RemoveAll(x => x.Id == id);
        }

        public bool Update(Trip item)
        {
            Trip curr = _trips.FirstOrDefault(x => x.Id == item.Id);
            if (curr == null)
                return false;

            curr.Name = item.Name;
            curr.Description = item.Description;
            curr.DateFrom = item.DateFrom;
            curr.DateTo = item.DateTo;
            curr.IsPrivate = item.IsPrivate;

            return true;
        }
    }
}