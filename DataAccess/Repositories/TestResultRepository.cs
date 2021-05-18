using DataAccess.Entities.Answers;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class TestResultRepository : CommonRepository<TestResult>, ITestResultRepository
    {
        private readonly AppContext context;

        public TestResultRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
