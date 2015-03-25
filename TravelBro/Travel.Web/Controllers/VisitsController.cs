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
    public class VisitsController : ApiController
    {
        // TODO: implement authentification validation

        // GET api/Visits
        public IEnumerable<Visit> GetVisits()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                return uow.Repo<VisitRepo>().GetAll();
            }
        }

        // GET api/Visits/5
        public IHttpActionResult GetVisit(int id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Visit visit = uow.Repo<VisitRepo>().Get(id);
                if (visit == null)
                {
                    return NotFound();
                }

                return Ok(visit);
            }
        }

        // PUT api/Visits/5
        [Authorize]
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

            using (IUnitOfWork uow = new UnitOfWork())
            {
                if (uow.Repo<TripRepo>().Get(visit.TripId).Author.Id != User.Identity.GetUserId())
                {
                    return StatusCode(HttpStatusCode.Unauthorized);
                }

                try
                {
                    uow.Repo<VisitRepo>().Update(visit);
                    uow.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(uow, id))
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

        // POST api/Visits
        [Authorize]
        public IHttpActionResult PostVisit(Visit visit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                if (uow.Repo<TripRepo>().Get(visit.TripId).Author.Id != User.Identity.GetUserId())
                {
                    return StatusCode(HttpStatusCode.Unauthorized);
                }

                uow.Repo<VisitRepo>().Insert(visit);
                uow.Commit();

                return CreatedAtRoute("DefaultApi", new { id = visit.Id }, new VisitDTO(visit));
            }
        }

        // DELETE api/Visits/5
        [Authorize]
        public IHttpActionResult DeleteVisit(int id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Visit visit = uow.Repo<VisitRepo>().Get(id);
                if (visit == null)
                {
                    return NotFound();
                }

                if (uow.Repo<TripRepo>().Get(visit.TripId).Author.Id != User.Identity.GetUserId())
                {
                    return StatusCode(HttpStatusCode.Unauthorized);
                }

                uow.Repo<VisitRepo>().Delete(visit);
                uow.Commit();

                return Ok(visit);
            }
        }

        private bool VisitExists(IUnitOfWork uow, int id)
        {
            return uow.Repo<VisitRepo>().FetchAll().Count(e => e.Id == id) > 0;
        }
    }
}