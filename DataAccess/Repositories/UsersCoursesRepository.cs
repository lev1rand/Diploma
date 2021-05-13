using DataAccess.Entities.ManyToManyEntities;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class UsersCoursesRepository : CommonRepository<UsersCourses>, IUsersCoursesRepository
    {
        private readonly AppContext context;

        public UsersCoursesRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}