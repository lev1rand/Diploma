using DataAccess.Interfaces;
using DataAccess.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class CodeRepository : IRepository<Code, int>
    {
        #region 

        private readonly TestContext context;

        #endregion

        public CodeRepository(TestContext context)
        {
            this.context = context;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Create(Code item)
        {
            context.Codes.Add(item);
        }

        public void Update(Code item)
        {
            context.Codes.Update(item);
        }

        public Code Get(int id)
        {
            return context.Codes
                .FirstOrDefault(p => p.Id == id);
        }

        public void Delete(int id)
        {
            Code deleted = context.Codes.Find(id);
            if (deleted != null)
            {
                context.Codes.Remove(deleted);
            }
        }

        public IQueryable<Code> GetAll()
        {
            return context.Codes;
        }

        public List<Code> GetByPredicate(Expression<Func<Code, bool>> predicate = null)
        {
            return null;
        }
    }
}
