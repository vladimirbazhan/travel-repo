using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.Repositories;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    public class PhotoController : ApiController
    {
        private readonly PhotoFileNameProvider _fileNameProvider = new PhotoFileNameProvider();

        private readonly Dictionary<string, string> _extensions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {".jpg", "image/jpeg"},
            {".png", "image/png"},
            {".gif", "image/gif"}
        };

        #region Actions
        [Route("api/trips/{tripId}/photos")]
        [HttpPost]
        [Authorize]
        public async Task<HttpResponseMessage> PostTripPhotoAsync(int tripId)
        {
            return await PostPhotoAsyncImpl(tripId, "trip", 0);
        }

        [Route("api/trips/{tripId}/{tripItemType}/{tripItemId}/photos")]
        [HttpPost]
        [Authorize]
        public async Task<HttpResponseMessage> PostTripItemPhotoAsync(int tripId, string tripItemType, int tripItemId)
        {
            return await PostPhotoAsyncImpl(tripId, tripItemType, tripItemId);
        }

        [Route("api/trips/photos/{photoId}")]
        [HttpGet]
        public HttpResponseMessage PhotosGet(string photoId)
        {
            string path = PhotoFileNameProvider.FileSaveLocation + photoId;
            return GetImageInternal(path);
        }

        

        [Route("api/trips/thumbnails/{photoId}")]
        [HttpGet]
        public HttpResponseMessage ThumbnailGet(string photoId)
        {
            string path = PhotoFileNameProvider.FileSaveLocationThumbnails + photoId;
            return GetImageInternal(path);
        }

        #endregion

        #region Private methods

        public async Task<HttpResponseMessage> PostPhotoAsyncImpl(int tripId, string tripItemType, int tripItemId)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                string usrId = User.Identity.GetUserId();
                var usr = uow.Repo<UserRepo>().Users.FirstOrDefault(x => x.Id == usrId);

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
                    List<Photo> photos = new List<Photo>();
                    foreach (var file in provider.Contents)
                    {
                        string fileName = _fileNameProvider.GetNewFileName(".jpg");
                        using (file)
                        {
                            using (Stream fileStream = await file.ReadAsStreamAsync())
                            using (Image img = new Bitmap(fileStream))
                            {
                                Size convertedSize = GetSizeWithDimensionsNotLongerThan(img.Width, img.Height, 1024, 768);
                                Size thumbNailSize = GetSizeWithDimensionsNotLongerThan(img.Width, img.Height, 250, 250);
                                using (Image convertedImage = ImageHelper.ResizeImage(img, convertedSize.Width, convertedSize.Height))
                                {
                                    //Save converted image
                                    ImageHelper.SaveJpeg(PhotoFileNameProvider.FileSaveLocation + fileName,
                                        convertedImage, 70);
                                }
                                using (Image thumbNailImage = ImageHelper.ResizeImage(img, thumbNailSize.Width, thumbNailSize.Height))
                                {
                                    //Save converted image
                                    ImageHelper.SaveJpeg(PhotoFileNameProvider.FileSaveLocationThumbnails + fileName,
                                        thumbNailImage, 70);
                                }
                            }
                        }

                        Photo photo = new Photo()
                        {
                            Author = usr,
                            Published = DateTime.Now,
                            ImagePath = fileName
                        };
                        photos.Add(photo);
                    }

                    switch (tripItemType)
                    {
                        case "trip":
                            uow.Repo<TripRepo>().AddPhotos(tripId, photos);
                            break;
                        case "visit":
                            uow.Repo<VisitRepo>().AddPhotos(tripItemId, photos);
                            break;
                        case "route":
                            uow.Repo<RouteRepo>().AddPhotos(tripItemId, photos);
                            break;
                    }
                    
                    uow.Commit();

                    // Send OK Response along with saved file names to the client. 
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
        }

        private HttpResponseMessage GetImageInternal(string path)
        {
            if (!File.Exists(path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            using (var fileStream = File.OpenRead(path))
            {
                byte[] fileData = new byte[fileStream.Length];
                var readResult = fileStream.ReadAsync(fileData, 0, (int)fileStream.Length);
                if (readResult.Result == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
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


        private Size GetSizeWithDimensionsNotLongerThan(int originalWidht, int originalHeight,
                                                         int maxResultWidth, int maxResultHeight)
        {

            double koefA = ((double)originalWidht) / ((double)maxResultWidth);
            double koefB = ((double)originalHeight) / ((double)maxResultHeight);

            double koef = koefA > koefB ? koefA : koefB;

            return new Size((int)Math.Round(originalWidht/koef, MidpointRounding.AwayFromZero),
                            (int)Math.Round(originalHeight / koef, MidpointRounding.AwayFromZero));
        }

        #endregion
    }
}
