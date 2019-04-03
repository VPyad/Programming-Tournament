using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        TEntity Get(TEntity entity);
        TEntity FirstOfDefault(Expression<Func<TEntity, bool>> exp);

        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> exp);
        IEnumerable<TEntity> GetAll();

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> list);

        void Update(TEntity entity);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> list);
    }
}
