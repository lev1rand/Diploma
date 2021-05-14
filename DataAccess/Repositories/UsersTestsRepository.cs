using DataAccess.Entities.ManyToManyEntities;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class UsersTestsRepository : CommonRepository<UsersTests>, IUsersTestsRepository
    {
        private readonly AppContext context;

        public UsersTestsRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}