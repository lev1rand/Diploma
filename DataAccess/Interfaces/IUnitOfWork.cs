using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<Code, int> Codes { get; }

        void Save();
    }
}
