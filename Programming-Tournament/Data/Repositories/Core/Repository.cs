using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext context;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> list)
        {
            context.Set<TEntity>().AddRange(list);
        }

        public TEntity FirstOfDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> exp)
        {
            return context.Set<TEntity>().FirstOrDefault(exp);
        }

        public TEntity Get(int id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public TEntity Get(TEntity entity)
        {
            return context.Set<TEntity>().First(x => x == entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> list)
        {
            context.Set<TEntity>().RemoveRange(list);
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }

        public IEnumerable<TEntity> Where(System.Linq.Expressions.Expression<Func<TEntity, bool>> exp)
        {
            return context.Set<TEntity>().Where(exp).ToList();
        }
    }
}
