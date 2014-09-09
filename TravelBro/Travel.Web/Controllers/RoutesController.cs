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
    public class RoutesController : ApiController
    {
        static private ITripRepo _tripRepo = new TripRepo();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET api/Routes
        public IQueryable<Route> GetRoutes()
        {
            return db.Routes;
        }

        // GET api/Routes/5
        [ResponseType(typeof(Route))]
        public IHttpActionResult GetRoute(int id)
        {
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return NotFound();
            }

            return Ok(route);
        }

        // PUT api/Routes/5
        public IHttpActionResult PutRoute(int id, Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_tripRepo.Get(route.TripId).Author.Id != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            if (id != route.Id)
            {
                return BadRequest();
            }

            db.Entry(route).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteExists(id))
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

        // POST api/Routes
        [ResponseType(typeof(Route))]
        public IHttpActionResult PostRoute(Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_tripRepo.Get(route.TripId).Author.Id != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            db.Routes.Add(route);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = route.Id }, route);
        }

        // DELETE api/Routes/5
        [ResponseType(typeof(Route))]
        public IHttpActionResult DeleteRoute(int id)
        {
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return NotFound();
            }

            if (_tripRepo.Get(route.TripId).Author.Id != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            db.Routes.Remove(route);
            db.SaveChanges();

            return Ok(route);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RouteExists(int id)
        {
            return db.Routes.Count(e => e.Id == id) > 0;
        }
    }
}