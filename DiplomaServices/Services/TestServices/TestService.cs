using DataAccess;
using DataAccess.Entities.ManyToManyEntities;
using DataAccess.Entities.TestEntities;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;
using DiplomaServices.Models;
using System;
using System.Collections.Generic;

namespace DiplomaServices.Services.TestServices
{
    public class TestService : ITestService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly ICourseService courseService;

        private readonly IQuestionService questionsService;

        private readonly IEmailService emailService;

        private readonly IUserService userService;

        private readonly IAnswersService answersService;

        private readonly MapperService mapper;

        #endregion

        public TestService(IUnitOfWork uow,
            ICourseService courseService,
            IQuestionService questionsService,
            IEmailService emailService,
            IUserService userService,
            IAnswersService answersService)
        {
            this.uow = uow;
            this.courseService = courseService;
            this.questionsService = questionsService;
            this.emailService = emailService;
            this.userService = userService;
            this.answersService = answersService;

            mapper = new MapperService();
        }
        public CreateTestModel CreateTest(CreateTestModel model)
        {
            var test = new Test();

            //Bind Course to Test
            if (courseService.CheckIfCourseExists(model.CourseId.Value))
            {
                test.CourseId = model.CourseId.Value;
            }
            else
            {
                throw new Exception(string.Format("Course with id {0} doesn't exist!", model.CourseId));
            }

            test.ComplitionDate = model.DateTime;
            test.Theme = model.Theme;
            uow.Tests.Create(test);
            uow.Save();

            //Create Questions
            if (model.Questions.Count > 0)
            {
                foreach (var question in model.Questions)
                {
                    question.TestId = test.Id;

                    questionsService.CreateQuestion(question);
                }
            }
            else
            {
                throw new Exception("Test doesn't contain questions!");
            }

            //Add Applicants
            if (model.Applicants.Count > 0)
            {
                AddApplicants(model.Applicants, test.Id);
            }

            //Notify Applicants about Test
            if (model.Applicants.Count > 0)
            {
                foreach (var applicant in model.Applicants)
                {
                    var isExistResponse = userService.CheckIfUserExists(applicant.Login);

                    if (isExistResponse.IsFound)
                    {
                        string messageText = string.Format("You have been subscribed to the test in the TestOn system, which will be hold on {0}. " +
                            "Test theme: {1}.", model.DateTime, model.Theme);

                        emailService.SendMessage(applicant.Login, messageText, null);
                    }
                }
            }

            return null;
        }
        public IEnumerable<Test> GetAll()
        {
            return uow.Tests.GetAll();
        }

        public IEnumerable<GetTestDetailsModel> GetTestDetailsByStudentId(int userId)
        {
            return null;
        }
        public void ProcessAnswers(List<SaveUserAnswersModel> answers, int userId)
        {
            answersService.SaveAnswers(answers, userId);
            answersService.EvaluateAnswers(answers, userId);
        }
        private void AddApplicants(List<CreateApplicantModel> applicants, int testId)
        {
            foreach (var applicant in applicants)
            {
                var isExistResponse = userService.CheckIfUserExists(applicant.Login);

                if (isExistResponse.IsFound)
                {
                    var user = uow.Users.Get(u => u.Login == applicant.Login);

                    uow.UsersTests.Create(new UsersTests { UserId = user.Id, TestId = testId });

                    uow.Save();
                }

            }
        }
    }
}
