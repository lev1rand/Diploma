using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class CommonRepository<T> : IRepository<T> where T : class
    {
        #region 

        private readonly AppContext context;
        protected readonly DbSet<T> dbSet;

        #endregion

        public CommonRepository(AppContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public void Create(T item)
        {
            context.Set<T>().Add(item);
        }

        public void Update(T item)
        {
            context.Set<T>().Update(item);
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.FirstOrDefault();
        }

        public List<T> GetByPredicate(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }

        public List<T> GetPaginated(int pageNumber, int pageSize)
        {
            return context.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void Delete(int id)
        {
            T deleted = context.Set<T>().Find(id);
            if (deleted != null)
            {
                context.Set<T>().Remove(deleted);
            }
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }
    }
}
