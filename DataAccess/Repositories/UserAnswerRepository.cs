using DataAccess.Entities.Answers;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class UserAnswerRepository : CommonRepository<UserAnswer>, IUserAnswerRepository
    {
        private readonly AppContext context;

        public UserAnswerRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
