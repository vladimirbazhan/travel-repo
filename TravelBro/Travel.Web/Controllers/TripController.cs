using System;
using System.Collections.Generic;
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
            item.DateFrom = DateTime.Now;
            item.DateTo = DateTime.Now;
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
    }
}
