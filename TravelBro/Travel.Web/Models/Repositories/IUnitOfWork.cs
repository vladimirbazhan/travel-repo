using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        TRepo Repo<TRepo>() where TRepo : RepositoryBase;
        void Commit();
    }
}