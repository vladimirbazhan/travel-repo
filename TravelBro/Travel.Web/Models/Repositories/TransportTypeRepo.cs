using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class TransTypeRepo : Repository<WebApplication1.Models.EntityModels.TransType>
    {
        public TransTypeRepo(ApplicationDbContext context, IUnitOfWork parent)
            :base(context, parent)
        { }
    }
}