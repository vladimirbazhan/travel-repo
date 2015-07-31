using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Models.EntityModels;

namespace WebApplication1.Models.IdentityModels
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string VisibleName { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        [DefaultValue(LanguageEnum.English)]
        public int? LanguageId { get; set; }
        private Language language;
        [ForeignKey("LanguageId")]
        public virtual Language Language
        {
            get { return language; }
            set
            {
                language = value;
                LanguageId = (value != null) ? value.Id : (int?)null;
            }
        }

        public void Merge(ApplicationUser user)
        {
            Name = user.Name;
            Surname = user.Surname;
            Patronymic = user.Patronymic;
            //Language = user.Language;
        }

        public virtual Collection<Trip> MemberInTrips { get; private set; }
        public virtual Collection<Visit> MemberInVisits { get; private set; }
        public virtual Collection<Route> MemberInRoutes { get; private set; }
    }
}