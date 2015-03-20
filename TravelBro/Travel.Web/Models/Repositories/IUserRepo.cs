using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public interface IUserRepo
    {
        IEnumerable<ApplicationUser> Users { get; } 
    }
}