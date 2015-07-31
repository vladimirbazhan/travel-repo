using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.Repositories;

namespace WebApplication1.Controllers
{
    public class LanguageController : ApiController
    {
        // GET api/Languages
        public IEnumerable<Language> GetLanguages()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                return uow.Repo<LanguageRepo>().GetAll();
            }
        }
    }
}
