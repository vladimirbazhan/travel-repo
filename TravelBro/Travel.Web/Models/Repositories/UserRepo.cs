using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class UserRepo : RepositoryBase
    {
        protected DbSet<ApplicationUser> dbSet;
        protected IUnitOfWork parent;

        private ApplicationDbContext context;

        public UserRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        {
            this.context = context;
            this.parent = parent;
            dbSet = context.Set<ApplicationUser>();
        }

        public IEnumerable<ApplicationUser> Users
        {
            get { return dbSet.ToList(); }
        }
    }
}