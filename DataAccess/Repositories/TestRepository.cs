using DataAccess.Entities.TestEntities;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class TestRepository : CommonRepository<Test>, ITestRepository
    {
        private readonly AppContext context;

        public TestRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
