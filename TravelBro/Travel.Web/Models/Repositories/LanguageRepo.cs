using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class LanguageRepo : Repository<WebApplication1.Models.EntityModels.Language>
    {
        public LanguageRepo(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        { }
    }
}