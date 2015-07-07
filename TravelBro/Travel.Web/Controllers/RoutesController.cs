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
using WebApplication1.Models.Repositories;

namespace WebApplication1.Controllers
{
    public class RoutesController : ApiController
    {
        // TODO: implement authentification validation

        // GET api/Routes
        public IEnumerable<Route> GetRoutes()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                return uow.Repo<RouteRepo>().GetAll();
            }
        }

        // GET api/Routes/5
        [ResponseType(typeof(Route))]
        public IHttpActionResult GetRoute(int id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Route route = uow.Repo<RouteRepo>().Get(id);
                if (route == null)
                {
                    return NotFound();
                }

                return Ok(route);
            }
        }

        // PUT api/Routes/5
        public IHttpActionResult PutRoute(int id, Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                if (uow.Repo<TripRepo>().Get(route.TripId).Author.Id != User.Identity.GetUserId())
                {
                    return StatusCode(HttpStatusCode.Unauthorized);
                }

                if (id != route.Id)
                {
                    return BadRequest();
                }
                
                try
                {
                    Route curr = uow.Repo<RouteRepo>().Get(route.Id);
                    curr.Merge(route);
                    uow.Repo<RouteRepo>().Update(curr);
                    uow.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(uow, id))
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
        }

        // POST api/Routes
        [ResponseType(typeof(Route))]
        public IHttpActionResult PostRoute(Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                if (uow.Repo<TripRepo>().Get(route.TripId).Author.Id != User.Identity.GetUserId())
                {
                    return StatusCode(HttpStatusCode.Unauthorized);
                }

                uow.Repo<RouteRepo>().Insert(route);
                uow.Commit();
                
                return CreatedAtRoute("DefaultApi", new { id = route.Id }, new RouteDTO(route));
            }
        }

        // DELETE api/Routes/5
        [ResponseType(typeof(Route))]
        public IHttpActionResult DeleteRoute(int id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Route route = uow.Repo<RouteRepo>().Get(id);
                if (route == null)
                {
                    return NotFound();
                }

                if (uow.Repo<TripRepo>().Get(route.TripId).Author.Id != User.Identity.GetUserId())
                {
                    return StatusCode(HttpStatusCode.Unauthorized);
                }

                uow.Repo<RouteRepo>().Delete(route);
                uow.Commit();

                return Ok(route);
            }
        }

        private bool RouteExists(IUnitOfWork uow, int id)
        {
            return uow.Repo<RouteRepo>().FetchAll().Count(e => e.Id == id) > 0;
        }
    }
}