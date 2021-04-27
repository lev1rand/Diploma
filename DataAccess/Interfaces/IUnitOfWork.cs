using DataAccess.Interfaces;
using DataAccess.Entities;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<Code, int> Codes { get; }
        IRepository<User, int> Users { get; }
        void Save();
    }
}
