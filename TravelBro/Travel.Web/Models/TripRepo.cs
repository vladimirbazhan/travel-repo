﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models
{
    public class TripRepo : ITripRepo
    {
        public TripRepo()
        {
            _context = ApplicationDbContext.GetInstance();
        }

        private ApplicationDbContext _context;

        public IEnumerable<Trip> GetAll()
        {
            return _context.Trips.ToList();
        }

        public Trip Get(int id)
        {
            return _context.Trips.FirstOrDefault(t => t.Id == id);
        }

        public Trip Add(Trip item)
        {
            _context.Trips.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip != null)
                _context.Trips.Remove(trip);
        }

        public bool Update(Trip item)
        {
            Trip curr = _context.Trips.FirstOrDefault(x => x.Id == item.Id);
            if (curr == null)
                return false;

            curr.Name = item.Name;
            curr.Description = item.Description;
            curr.DateFrom = item.DateFrom;
            curr.DateTo = item.DateTo;

            return true;
        }
    }
}