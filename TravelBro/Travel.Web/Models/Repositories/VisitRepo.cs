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
    }
}