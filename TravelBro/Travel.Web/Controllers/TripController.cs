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

        static private ITripRepo _repo = new TripRepo();
        private FileNameProvider _fileNameProvider = new FileNameProvider();

        private Dictionary<string, string> _extensions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {".jpg", "image/jpeg"},
            {".png", "image/png"},
            {".gif", "image/gif"}
        };

        public IEnumerable<TripDTO> GetTrips()
        {
            var trips = _repo.GetAll().Where(x => (!x.IsPrivate || x.Author.Id == User.Identity.GetUserId())).ToList();
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
            if (_repo.Get(id).Author.Id != User.Identity.GetUserId())
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

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
            if (item.Author.Id != User.Identity.GetUserId())
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
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
            }

            Comment res = _repo.AddComment(tripId, comment);
            var response = Request.CreateResponse<CommentDTO>(HttpStatusCode.Created, new CommentDTO(res));

            string uri = Url.Link("GetTripCommentById", new { tripId = tripId, id = res.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [Route("api/trips/{tripId}/photos")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostPhotoAsync(int tripId)
        {
            string usrId = User.Identity.GetUserId();
            var usr = ApplicationDbContext.GetInstance().Users.FirstOrDefault(x => x.Id == usrId);
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

                _repo.AddPhotos(tripId, tripPhotos);

                // Send OK Response along with saved file names to the client. 
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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

        private string GetFileHash(string fileName)
        {
            using (FileStream fStream = File.OpenRead(fileName))
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(fStream);

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
