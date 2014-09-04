using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EntityModels;

namespace WebApplication1.Models.IdentityModels
{
    public class ApplicationUserDTO
    {
        public ApplicationUserDTO(ApplicationUser user)
        {
            VisibleName = user.VisibleName;
            Email       = user.Email;
        }

        public string VisibleName { get; set; }
        public string Email { get; set; }
    }
}