using DataAccess.Entities.Answers;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class RightSimpleAnswerRepository : CommonRepository<RightSimpleAnswer>, IRightSimpleAnswerRepository
    {
        private readonly AppContext context;

        public RightSimpleAnswerRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
