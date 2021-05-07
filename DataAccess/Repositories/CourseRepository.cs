using DataAccess.Entities;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class CourseRepository : CommonRepository<Course>, ICourseRepository
    {
        private readonly AppContext context;

        public CourseRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
