using DataAccess.Entities.TestEntities;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class ResponseOptionRepository : CommonRepository<ResponseOption>, IResponseOptionRepository
    {
        private readonly AppContext context;

        public ResponseOptionRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
