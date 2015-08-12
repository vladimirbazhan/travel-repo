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
            PasswordChangedUtc = user.PasswordChangedUtc;
            Name = user.Name;
            Surname = user.Surname;
            Patronymic = user.Patronymic;
            Language = user.Language;
        }

        public string VisibleName { get; set; }
        public string Email { get; set; }

        public DateTime PasswordChangedUtc { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public Language Language { get; set; }
    }
}