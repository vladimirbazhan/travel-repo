using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class RouteRepo : Repository<Route>
    {
        public RouteRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        {
        }
    }
}