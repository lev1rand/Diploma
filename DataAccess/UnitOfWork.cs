using DataAccess.Repositories;
using System;
using DataAccess.Interfaces.Repositories;

namespace DataAccess
{
    public class UnitOfWork: IUnitOfWork
    {
        #region privateMembers

        private AppContext context;

        private IUserRepository userRepository;
        private ICourseRepository courseRepository;
        private IUserAnswerRepository userAnswersRepository;
        private ITestRepository testRepository;
        private IQuestionRepository questionRepository;
        private IRightSimpleAnswerRepository rightSimpleAnswerRepository;
        private IResponseOptionRepository responseOptionRepository;
        private IUsersCoursesRepository usersCoursesRepository;

        private bool isDisposed;

        #endregion

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(context);

                return userRepository;
            }
        }

        public ICourseRepository Courses
        {
            get
            {
                if (courseRepository == null)
                    courseRepository = new CourseRepository(context);

                return courseRepository;
            }
        }

        public ITestRepository Tests
        {
            get
            {
                if (testRepository == null)
                    testRepository = new TestRepository(context);

                return testRepository;
            }
        }

        public IQuestionRepository Questions
        {
            get
            {
                if (questionRepository == null)
                    questionRepository = new QuestionRepository(context);

                return questionRepository;
            }
        }

        public IResponseOptionRepository ResponseOptions
        {
            get
            {
                if (responseOptionRepository == null)
                    responseOptionRepository = new ResponseOptionRepository(context);

                return responseOptionRepository;
            }
        }

        public IRightSimpleAnswerRepository RightSimpleAnswers
        {
            get
            {
                if (rightSimpleAnswerRepository == null)
                    rightSimpleAnswerRepository = new RightSimpleAnswerRepository(context);

                return rightSimpleAnswerRepository;
            }
        }

        public IUserAnswerRepository UserAnswers
        {
            get
            {
                if (userAnswersRepository == null)
                    userAnswersRepository = new UserAnswerRepository(context);

                return userAnswersRepository;
            }
        }

        public IUsersCoursesRepository UsersCourses
        {
            get
            {
                if (usersCoursesRepository == null)
                    usersCoursesRepository = new UsersCoursesRepository(context);

                return usersCoursesRepository;
            }
        }

        public UnitOfWork(AppContext context)
        {
            this.context = context;
            isDisposed = false;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}
