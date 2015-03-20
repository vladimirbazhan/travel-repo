using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.IdentityModels;


namespace WebApplication1.Models.Repositories
{
    public class Repository<TEntity> : RepositoryBase, IRepository<TEntity> where TEntity : Entity
    {
        protected IDbSet<TEntity> dbSet; 

        public Repository(ApplicationDbContext context)
            :base(context)
        {
            this.dbSet = this.context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual TEntity Get(int id)
        {
            return dbSet.Where(x => x.Id == id).FirstOrDefault();
        }

        public virtual TEntity Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
            return Get(entity.Id);
        }

        public virtual bool Update(TEntity entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            TEntity entity = dbSet.FirstOrDefault(x => x.Id == id);
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