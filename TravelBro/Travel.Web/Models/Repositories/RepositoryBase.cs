using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class RepositoryBase
    {
        protected ApplicationDbContext context;
        public RepositoryBase(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}