using DataAccess.Entities.Test;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class QuestionRepository : CommonRepository<Question>, IQuestionRepository
    {
        private readonly AppContext context;

        public QuestionRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
