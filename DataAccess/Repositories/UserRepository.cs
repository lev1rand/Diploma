using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class UserRepository : IRepository<User, int>
    {
        #region 

        private readonly AppContext context;

        #endregion

        public UserRepository(AppContext context)
        {
            this.context = context;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Create(User item)
        {
            context.Users.Add(item);
        }

        public void Update(User item)
        {
            context.Users.Update(item);
        }

        public User Get(int id)
        {
            return context.Users
                .FirstOrDefault(p => p.Id == id);
        }

        public List<User> GetByPredicate(Expression<Func<User, bool>> predicate = null)
        {
            IQueryable<User> query = context.Set<User>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }
        public void Delete(int id)
        {
            User deleted = context.Users.Find(id);
            if (deleted != null)
            {
                context.Users.Remove(deleted);
            }
        }

        public IQueryable<User> GetAll()
        {
            return context.Users;
        }


    }
}
