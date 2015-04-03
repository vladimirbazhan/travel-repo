using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.Repositories
{
    public class Repository<TEntity> : RepositoryBase, IRepository<TEntity> where TEntity : Entity
    {
        protected DbSet<TEntity> dbSet;
        protected IUnitOfWork parent;

        protected ApplicationDbContext context;

        public Repository(ApplicationDbContext context, IUnitOfWork parent)
            : base(context, parent)
        {
            this.context = context;
            this.parent = parent;
            dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual TEntity Get(int id)
        {
            return dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return dbSet.Add(entity);
        }

        public virtual bool Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(int id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public virtual IQueryable<TEntity> FetchAll()
        {
            return dbSet;
        }
    }
}