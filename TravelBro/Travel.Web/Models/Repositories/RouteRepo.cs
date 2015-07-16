using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class RouteRepo : OrderedItemRepo<Route>
    {
        public RouteRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        {
        }

        public override Route Insert(Route entity)
        {
            if (entity.TransType != null)
            {
                context.TransTypes.Attach(entity.TransType);
            }
            return base.Insert(entity);
        }

        public void AddPhotos(int routeId, IEnumerable<Photo> photos)
        {
            Route curr = Get(routeId);
            if (curr.Photos == null)
            {
                curr.Photos = new Collection<Photo>();
            }
            foreach (Photo photo in photos)
            {
                curr.Photos.Add(photo);
            }
        }

        public void AddComment(int routeId, Comment comment)
        {
            Route curr = Get(routeId);
            
            if (curr.Comments == null)
            {
                curr.Comments = new Collection<Comment>();
            }

            curr.Comments.Add(comment);
        }

        public override void Delete(int id)
        {
            base.Delete(id);
            parent.Commit();
            parent.Repo<PhotoRepo>().ClearUnusedPhotos();
        }
    }
}