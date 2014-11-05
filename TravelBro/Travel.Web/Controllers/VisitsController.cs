using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Controllers
{
    public class VisitsController : ApiController
    {
        // TODO: implement authentification validation
        // TODO: wrap ApplicationDbContext with using statements

        private ApplicationDbContext db = new ApplicationDbContext();
        static private ITripRepo _tripRepo = new TripRepo();

        // GET api/Visits
        public IQueryable<Visit> GetVisits()
        {
            return db.Visits;
        }

        // GET api/Visits/5
        [ResponseType(typeof(Visit))]
        public IHttpActionResult GetVisit(int id)
        {
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return NotFound();
            }

            return Ok(visit);
        }

        // PUT api/Visits/5
        public IHttpActionResult PutVisit(int id, Visit visit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != visit.Id)
            {
                return BadRequest();
            }

            if (_tripRepo.Get(visit.TripId).Author.Id != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            db.Entry(visit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Visits
        [ResponseType(typeof(Visit))]
        public IHttpActionResult PostVisit(Visit visit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_tripRepo.Get(visit.TripId).Author.Id != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            db.Visits.Add(visit);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = visit.Id }, visit);
        }

        // DELETE api/Visits/5
        [ResponseType(typeof(Visit))]
        public IHttpActionResult DeleteVisit(int id)
        {
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return NotFound();
            }

            if (_tripRepo.Get(visit.TripId).Author.Id != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            db.Visits.Remove(visit);
            db.SaveChanges();

            return Ok(visit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VisitExists(int id)
        {
            return db.Visits.Count(e => e.Id == id) > 0;
        }
    }
}