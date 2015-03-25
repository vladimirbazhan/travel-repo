using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;
        public UnitOfWork()
        {
            this.context = new ApplicationDbContext();
        }

        #region Destructor/Dispose
        ~UnitOfWork()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                    GC.SuppressFinalize(this);
                }
            }
        }
        #endregion
        
        Dictionary<Type, object> _repos = new Dictionary<Type, object>();
        public TRepo Repo<TRepo>() where TRepo : RepositoryBase
        {
            object outRepo;
            if (!_repos.TryGetValue(typeof (TRepo), out outRepo))
            {
                outRepo = (TRepo)Activator.CreateInstance(typeof(TRepo), context, this);
                _repos.Add(typeof(TRepo), outRepo);
            }
            return (TRepo) outRepo;
        }


        public void Commit()
        {
            context.SaveChanges();
        }
    }
}