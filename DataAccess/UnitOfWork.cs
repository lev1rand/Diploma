using DataAccess.Interfaces;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;

namespace DataAccess
{
    public class UnitOfWork: IUnitOfWork
    {
        #region privateMembers

        private AppContext context;

        private IRepository<User, int> userRepository;

        private bool isDisposed;

        #endregion

        public IRepository<User, int> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(context);

                return userRepository;
            }
        }

        public UnitOfWork(AppContext context)
        {
            this.context = context;
            isDisposed = false;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}
