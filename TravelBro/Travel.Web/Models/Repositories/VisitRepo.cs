using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class VisitRepo : OrderedItemRepo<Visit>
    {
        public VisitRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        {
        }

        public void AddPhotos(int visitId, IEnumerable<Photo> photos)
        {
            Visit curr = Get(visitId);
            if (curr.Photos == null)
            {
                curr.Photos = new Collection<Photo>();
            }
            foreach (Photo photo in photos)
            {
                curr.Photos.Add(photo);
            }
        }

        public void AddComment(int visitId, Comment comment)
        {
            Visit curr = Get(visitId);

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