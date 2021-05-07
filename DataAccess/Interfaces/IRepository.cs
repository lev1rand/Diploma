using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        public T Get(Expression<Func<T, bool>> predicate = null);
        List<T> GetByPredicate(Expression<Func<T, bool>> predicate = null);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
