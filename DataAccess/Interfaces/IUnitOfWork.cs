using DataAccess.Interfaces.Repositories;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ICourseRepository Courses { get; }
        ITestRepository Tests { get; }
        IQuestionRepository Questions { get; }
        IResponseOptionRepository ResponseOptions { get; }
        IRightSimpleAnswerRepository RightSimpleAnswers { get; }
        IUserAnswerRepository UserAnswers { get; }
        IUsersCoursesRepository UsersCourses { get; }
        void Save();
    }
}
