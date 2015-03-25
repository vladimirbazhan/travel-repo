using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class RepositoryBase
    {
        public RepositoryBase(ApplicationDbContext context, IUnitOfWork parent)
        {
        }
    }
}