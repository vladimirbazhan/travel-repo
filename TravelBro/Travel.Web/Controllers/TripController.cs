using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;
using WebApplication1.Models.Repositories;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    public class TripsController : ApiController
    {
        public IEnumerable<TripDTO> GetTrips()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                var trips = uow.Repo<TripRepo>().GetAll().Where(x => (!x.IsPrivate || x.Author.Id == User.Identity.GetUserId())).ToList();
                var tripsDTO = from trip in trips
                               select new TripDTO(trip);
                return tripsDTO.ToList();
            }
        }

        public IHttpActionResult GetTrip(int id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Trip res = uow.Repo<TripRepo>().Get(id);
                if (res == null)
                {
                    return NotFound();
                }
                if (res.IsPrivate && res.Author.Id != User.Identity.GetUserId())
                {
                    return Unauthorized();
                }
                return Ok(new TripDTO(res));
            }
        }

        [Authorize]
        public IHttpActionResult PostTrip(Trip item)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                string id = User.Identity.GetUserId();
                var usr = uow.Repo<UserRepo>().Users.FirstOrDefault(x => x.Id == id);

                if (usr == null)
                {
                    return Unauthorized();
                }

                item.Author = usr;
                Trip res = uow.Repo<TripRepo>().Insert(item);
                uow.Commit();

                return CreatedAtRoute("DefaultApi", new { id = res.Id }, new TripDTO(res));
            }
        }

        [Authorize]
        public void PutTrip(int id, Trip item)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Trip curr = uow.Repo<TripRepo>().Get(id);
                if (curr.Author.Id != User.Identity.GetUserId())
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                curr.Merge(item);
                
                if (!uow.Repo<TripRepo>().Update(curr))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                uow.Commit();
            }
        }

        [Authorize]
        public HttpResponseMessage DeleteTrip(int id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Trip item = uow.Repo<TripRepo>().Get(id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                if (item.Author.Id != User.Identity.GetUserId())
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                uow.Repo<TripRepo>().Delete(id);
                uow.Commit();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        [Route("api/trips/{tripId}/comments/{id}", Name = "GetTripCommentById")]
        [HttpGet]
        public CommentDTO GetComment(int tripId, int id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Trip trip = uow.Repo<TripRepo>().Get(tripId);
                if (trip == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                Comment comment = trip.Comments.Where(x => x.Id == id).FirstOrDefault();
                if (comment != null)
                {
                    return new CommentDTO(comment);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }
        
        [Route("api/trips/{tripId}/comments")]
        [HttpPost]
        public HttpResponseMessage PostComment(int tripId, Comment comment)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                comment.Published = DateTime.Now;

                string usrId = User.Identity.GetUserId();
                var usr = uow.Repo<UserRepo>().Users.FirstOrDefault(x => x.Id == usrId);
                if (usr != null)
                {
                    comment.Author = usr;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                Comment res = uow.Repo<TripRepo>().AddComment(tripId, comment);
                uow.Commit();

                var response = Request.CreateResponse(HttpStatusCode.Created, new CommentDTO(res));

                string uri = Url.Link("GetTripCommentById", new { tripId = tripId, id = res.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
        }
        
    }
}
