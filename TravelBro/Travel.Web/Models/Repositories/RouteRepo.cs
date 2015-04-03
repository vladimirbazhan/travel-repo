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
    }
}