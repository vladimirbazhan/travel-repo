using System.Collections.Generic;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class UserRepositoryBase : RepositoryBase, IUserRepo
    {
        public UserRepositoryBase(ApplicationDbContext context)
            : base(context)
        {
        }

        public IEnumerable<ApplicationUser> Users
        {
            get { return context.Users; }
        }
    }
}