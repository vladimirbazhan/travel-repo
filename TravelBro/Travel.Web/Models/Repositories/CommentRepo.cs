using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class CommentRepo : Repository<Comment>
    {
        public CommentRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        {
        }
    }
}