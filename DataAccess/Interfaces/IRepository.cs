using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public T Get(Expression<Func<T, bool>> predicate = null);
        public List<T> GetByPredicate(Expression<Func<T, bool>> predicate = null);
        public List<T> GetPaginated(int pageNumber, int pageSize);
        public void Create(T item);
        public void Update(T item);
        public void Delete(int id);
    }
}
