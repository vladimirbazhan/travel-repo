using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;
using WebGrease.Css.Extensions;

namespace WebApplication1.Models.Repositories
{
    public class PhotoRepo : Repository<Photo>
    {
        public PhotoRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        { }

        public void ClearUnusedPhotos(ApplicationDbContext.ProcessPhotoDelegate processPhoto)
        {
            // TODO: performance issues could arise because we fetch too much data from DB, remember about it
            // all photos
            var photoes = dbSet.Include(x => x.PhotosToRoutes).Include(x => x.PhotosToTrips).Include(x => x.PhotosToVisits).ToList();
            // unused photos
            var unusedPhotos = photoes.Where(x => x.PhotosToRoutes.Count == 0 && x.PhotosToTrips.Count == 0 && x.PhotosToVisits.Count == 0);
            unusedPhotos.ForEach(x => processPhoto(x));
            dbSet.RemoveRange(unusedPhotos);
        }
    }
}