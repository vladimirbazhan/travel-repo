using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace WebApplication1.Controllers
{
    public class TripsController : ApiController
    {
        class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            private FileNameProvider _fnProvider;

            public CustomMultipartFormDataStreamProvider(FileNameProvider fnProvider)
                : base(fnProvider.FileSaveLocation)
            {
                _fnProvider = fnProvider;
            }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                
                string fileName = headers.ContentDisposition.FileName.Replace("\"", "");
                return _fnProvider.GetNewFileName(Path.GetExtension(fileName));
            }
        }

        class FileNameProvider
        {
            private readonly object _lock = new object();

            public string FileSaveLocation { get; private set; }

            public FileNameProvider()
            {
                FileSaveLocation = HttpContext.Current.Server.MapPath("~/App_Data/Images");
            }
            public string GetNewFileName(string extension)
            {
                lock (_lock)
                {
                    string fileName = Guid.NewGuid().ToString("N");
                    int counter = 0;
                    while (File.Exists(FileSaveLocation + "\\" + fileName + counter + extension))
                        counter++;
                    return fileName + counter + extension;
                }
            }
        }

        private FileNameProvider _fileNameProvider = new FileNameProvider();

        private Dictionary<string, string> _extensions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {".jpg", "image/jpeg"},
            {".png", "image/png"},
            {".gif", "image/gif"}
        };

        public IEnumerable<TripDTO> GetTrips()
        {
            using (var repo = new TripRepo())
            {
                var trips = repo.GetAll().Where(x => (!x.IsPrivate || x.Author.Id == User.Identity.GetUserId())).ToList();
                var tripsDTO = from trip in trips
                               select new TripDTO(trip);
                return tripsDTO.ToList();
            }
        }

        public IHttpActionResult GetTrip(int id)
        {
            using (var repo = new TripRepo())
            {
                Trip res = repo.Get(id);
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(new TripDTO(res));
            }
        }

        public IEnumerable<Trip> GetTripByName(string name)
        {
            using (var repo = new TripRepo())
            {
                return repo.GetAll().Where(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)).ToList(); 
                
            }
        }

        [Authorize]
        public HttpResponseMessage PostTrip(Trip item)
        {
            using (var repo = new TripRepo())
            {
                string id = User.Identity.GetUserId();
                var usr = repo.Context.Users.FirstOrDefault(x => x.Id == id);

                item.Author = usr;
                Trip res = repo.Add(item);
                var response = Request.CreateResponse<TripDTO>(HttpStatusCode.Created, new TripDTO(res));

                string uri = Url.Link("DefaultApi", new { Id = res.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
        }

        [Authorize]
        public void PutTrip(int id, Trip item)
        {
            using (var repo = new TripRepo())
            {
                if (repo.Get(id).Author.Id != User.Identity.GetUserId())
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                item.Id = id;

                if (!repo.Update(item))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        [Authorize]
        public void DeleteTrip(int id)
        {
            using (var repo = new TripRepo())
            {
                Trip item = repo.Get(id);
                if (item == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                if (item.Author.Id != User.Identity.GetUserId())
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                repo.Remove(id);
            }
        }

        [Route("api/trips/{tripId}/comments/{id}", Name = "GetTripCommentById")]
        [HttpGet]
        public CommentDTO GetComment(int tripId, int id)
        {
            using (var repo = new TripRepo())
            {
                Trip trip = repo.Get(tripId);
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
            using (var repo = new TripRepo())
            {
                comment.Published = DateTime.Now;

                string usrId = User.Identity.GetUserId();
                var usr = repo.Context.Users.FirstOrDefault(x => x.Id == usrId);
                if (usr != null)
                {
                    comment.Author = usr;
                }

                Comment res = repo.AddComment(tripId, comment);
                var response = Request.CreateResponse<CommentDTO>(HttpStatusCode.Created, new CommentDTO(res));

                string uri = Url.Link("GetTripCommentById", new { tripId = tripId, id = res.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
        }

        [Route("api/trips/{tripId}/photos")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostPhotoAsync(int tripId)
        {
            using (var repo = new TripRepo())
            {
                string usrId = User.Identity.GetUserId();
                var usr = repo.Context.Users.FirstOrDefault(x => x.Id == usrId);
                if (usr == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                // Check whether the POST operation is MultiPart? 
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider. 
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(_fileNameProvider);

                try
                {
                    await Request.Content.ReadAsMultipartAsync(provider);
                    List<Photo> tripPhotos = new List<Photo>();
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        Photo photo = new Photo()
                        {
                            Author = usr,
                            Published = DateTime.Now,
                            ImagePath = Path.GetFileName(file.LocalFileName)
                        };
                        tripPhotos.Add(photo);
                    }

                    repo.AddPhotos(tripId, tripPhotos);

                    // Send OK Response along with saved file names to the client. 
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
        }

        [Route("api/trips/photos/{photoId}")]
        [HttpGet]
        public async Task<HttpResponseMessage> PhotosGet(string photoId)
        {
            string fileSaveLocation = HttpContext.Current.Server.MapPath("~/App_Data/Images");
            string path = fileSaveLocation + "\\" + photoId;
            var fileStream = File.OpenRead(path);
            {
                var resp = new HttpResponseMessage()
                {
                    Content = new StreamContent(fileStream)
                };

                // Find the MIME type
                string ext = Path.GetExtension(path);
                string mimeType = _extensions[ext];
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                return resp;
            }
        }
    }
}
