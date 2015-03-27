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
    public class TransTypesController : ApiController
    {
        // GET api/Routes
        public IEnumerable<TransType> GetTransTypes()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                return uow.Repo<TransTypeRepo>().GetAll();
            }
        }
    }
}
