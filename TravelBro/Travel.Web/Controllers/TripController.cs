using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Controllers
{
    public class TripsController : ApiController
    {
        static private ITripRepo _repo = new TripRepo();

        public IEnumerable<TripDTO> GetTrips()
        {
            var trips = _repo.GetAll().ToList();
            var tripsDTO = from trip in trips
                select new TripDTO(trip);
            return tripsDTO;
        }

        public IHttpActionResult GetTrip(int id)
        {
            Trip res = _repo.Get(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(new TripDTO(res));
        }

        public IEnumerable<Trip> GetTripByName(string name)
        {
            return _repo.GetAll().Where(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        [Authorize]
        public HttpResponseMessage PostTrip(Trip item)
        {
            string id = User.Identity.GetUserId();
            var usr = ApplicationDbContext.GetInstance().Users.FirstOrDefault(x => x.Id == id);

            item.Author = usr;
            Trip res = _repo.Add(item);
            var response = Request.CreateResponse<TripDTO>(HttpStatusCode.Created, new TripDTO(res));

            string uri = Url.Link("DefaultApi", new { Id = res.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [Authorize]
        public void PutTrip(int id, Trip item)
        {
            item.Id = id;

            if (!_repo.Update(item))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Authorize]
        public void DeleteTrip(int id)
        {
            Trip item = _repo.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            _repo.Remove(id);
        }

        [Route("api/trips/{tripId}/comments/{id}", Name = "GetTripCommentById")]
        [HttpGet]
        public CommentDTO GetComment(int tripId, int id)
        {
            Trip trip = _repo.Get(tripId);
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
        
        [Route("api/trips/{tripId}/comments")]
        [HttpPost]
        public HttpResponseMessage PostComment(int tripId, Comment comment)
        {
            comment.Published = DateTime.Now;

            string usrId = User.Identity.GetUserId();
            var usr = ApplicationDbContext.GetInstance().Users.FirstOrDefault(x => x.Id == usrId);
            if (usr != null)
            {
                comment.Author = usr;
                comment.AuthorName = usr.UserName;
            }

            Comment res = _repo.AddComment(tripId, comment);
            var response = Request.CreateResponse<CommentDTO>(HttpStatusCode.Created, new CommentDTO(res));

            string uri = Url.Link("GetTripCommentById", new { tripId = tripId, id = res.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
    }
}
