using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebApplication1.Models;
using WebApplication1.Models.EntityModels;

namespace WebApplication1.Controllers
{
    public class TripsController : ApiController
    {
        static private ITripRepo _repo = new TripRepo();

        public IEnumerable<Trip> GetAllTrips()
        {
            return _repo.GetAll();
        }

        public IHttpActionResult GetTrip(int id)
        {
            Trip res = _repo.Get(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        public IEnumerable<Trip> GetTripByName(string name)
        {
            return _repo.GetAll().Where(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public HttpResponseMessage PostTrip(Trip item)
        {
            Trip res = _repo.Add(item);
            var response = Request.CreateResponse<Trip>(HttpStatusCode.Created, res);

            string uri = Url.Link("DefaultApi", new { Id = res.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutTrip(int id, Trip item)
        {
            item.Id = id;
            if (!_repo.Update(item))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

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
