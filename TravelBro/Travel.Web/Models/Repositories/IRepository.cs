using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models.Repositories
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        IQueryable<TEntity> FetchAll();
        TEntity Insert(TEntity entity);
        bool Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(int id);
    }
}