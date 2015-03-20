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
        class FileNameProvider
        {
            private readonly object _lock = new object();

            public string FileSaveLocation { get; private set; }

            public FileNameProvider()
            {
                FileSaveLocation = HttpContext.Current.Server.MapPath("~/App_Data/Images") + '\\';
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

        private readonly Dictionary<string, string> _extensions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {".jpg", "image/jpeg"},
            {".png", "image/png"},
            {".gif", "image/gif"}
        };

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
                var usr = uow.Repo<UserRepositoryBase>().Users.FirstOrDefault(x => x.Id == id);

                if (usr == null)
                {
                    return Unauthorized();
                }

                item.Author = usr;
                Trip res = uow.Repo<TripRepo>().Insert(item);

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

                // TODO: crappy solution, change it using DI service
                uow.Repo<TripRepo>().PhotoLocationPath = _fileNameProvider.FileSaveLocation;
                uow.Repo<TripRepo>().Delete(id);
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
                var usr = uow.Repo<UserRepositoryBase>().Users.FirstOrDefault(x => x.Id == usrId);
                if (usr != null)
                {
                    comment.Author = usr;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                Comment res = uow.Repo<TripRepo>().AddComment(tripId, comment);
                var response = Request.CreateResponse<CommentDTO>(HttpStatusCode.Created, new CommentDTO(res));

                string uri = Url.Link("GetTripCommentById", new { tripId = tripId, id = res.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
        }
        
        [Route("api/trips/{tripId}/photos")]
        [HttpPost]
        [Authorize]
        public async Task<HttpResponseMessage> PostPhotoAsync(int tripId)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                string usrId = User.Identity.GetUserId();
                var usr = uow.Repo<UserRepositoryBase>().Users.FirstOrDefault(x => x.Id == usrId);

                Trip trip = uow.Repo<TripRepo>().Get(tripId);
                if (trip.Author.Id != usrId)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                // Check whether the POST operation is MultiPart? 
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                try
                {
                    // Read all contents of multipart message into MultipartMemoryStreamProvider. 
                    MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();

                    await Request.Content.ReadAsMultipartAsync(provider);
                    List<Photo> tripPhotos = new List<Photo>();
                    foreach (var file in provider.Contents)
                    {
                        Stream fileStream = await file.ReadAsStreamAsync();
                        Image img = new Bitmap(fileStream);
                        img = ImageHelper.ResizeImage(img, 1024, 768);
                        string fileName = _fileNameProvider.GetNewFileName(".jpg");
                        ImageHelper.SaveJpeg(_fileNameProvider.FileSaveLocation  + fileName, img, 70);

                        Photo photo = new Photo()
                        {
                            Author = usr,
                            Published = DateTime.Now,
                            ImagePath = fileName
                        };
                        tripPhotos.Add(photo);
                    }

                    uow.Repo<TripRepo>().AddPhotos(tripId, tripPhotos);

                    // Send OK Response along with saved file names to the client. 
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
        }

        [Route("api/trips/photos/{photoId}")]
        [HttpGet]
        public HttpResponseMessage PhotosGet(string photoId)
        {
            string path = _fileNameProvider.FileSaveLocation + photoId;
            var fileStream = File.OpenRead(path);
            {
                byte[] fileData = new byte[fileStream.Length];
                var readResult = fileStream.ReadAsync(fileData, 0, (int)fileStream.Length);
                if (readResult.Result == 0)
                {
                    Request.CreateResponse(HttpStatusCode.NotFound);
                }

                var resp = new HttpResponseMessage()
                {
                    Content = new ByteArrayContent(fileData)
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
