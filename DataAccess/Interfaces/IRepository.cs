using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public T Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        public List<T> GetSeveral(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        public List<T> GetPaginated(int pageNumber, int pageSize, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        public void Create(T item);
        public void Update(T item);
        public void Delete(int id);
    }
}
