using DataAccess.Interfaces;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;

namespace DataAccess
{
    public class UnitOfWork: IUnitOfWork
    {
        #region privateMembers

        private TestContext context;

        private IRepository<Code, int> codeRepository;

        private IRepository<User, int> userRepository;

        private bool isDisposed;

        #endregion

        public IRepository<Code, int> Codes
        {
            get
            {
                if (codeRepository == null)
                    codeRepository = new CodeRepository(context);

                return codeRepository;
            }
        }

        public IRepository<User, int> Users
        {
            get
            {
                if (userRepository == null)
                    codeRepository = new CodeRepository(context);

                return userRepository;
            }
        }

        public UnitOfWork(TestContext context)
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
