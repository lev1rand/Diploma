using DataAccess.Interfaces;
using DataAccess.Entities;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<User, int> Users { get; }
        void Save();
    }
}
