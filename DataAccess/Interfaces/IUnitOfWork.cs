using DataAccess.Interfaces;
using DataAccess.Entities;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<Code, int> Codes { get; }

        void Save();
    }
}
