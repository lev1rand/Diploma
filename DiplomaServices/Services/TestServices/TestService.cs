using DataAccess;
using DataAccess.Entities.ManyToManyEntities;
using DataAccess.Entities.TestEntities;
using DiplomaServices.Interfaces;
using DiplomaServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace DiplomaServices.Services.TestServices
{
    public class TestService : ITestService
    {
        #region Fields

        private readonly IUnitOfWork uow;

        private readonly ICourseService courseService;

        private readonly IQuestionService questionsService;

        private readonly IEmailService emailService;

        private readonly IUserService userService;

        private readonly IAnswersService answersService;

        private readonly CachingService cachingService;

        #endregion

        #region Public methods

        public TestService(IUnitOfWork uow,
            ICourseService courseService,
            IQuestionService questionsService,
            IEmailService emailService,
            IUserService userService,
            IAnswersService answersService,
            IMemoryCache cache)
        {
            this.uow = uow;
            this.courseService = courseService;
            this.questionsService = questionsService;
            this.emailService = emailService;
            this.userService = userService;
            this.answersService = answersService;

            cachingService = new CachingService(cache);
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
                throw new Exception(string.Format("Курсу із id {0} не існує!", model.CourseId));
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
                throw new Exception("Тест не містить питань!");
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
                        string messageText = string.Format("Увага! Ти став підписником на тестування у системі TestOn, яке буде проведене {0}. " +
                            "Тестування за темою: {1}.", model.DateTime, model.Theme);

                        emailService.SendMessage(applicant.Login, messageText, null);
                    }
                }
            }

            return null;
        }
        public IEnumerable<GetTestSimpleModel> GetAll()
        {
            var testModels = new List<GetTestSimpleModel>();

            if (uow.Tests.GetAll() == null)
            {
                return testModels;
            }

            foreach (var test in uow.Tests.GetSeveral(include: t => t.Include(t => t.Course)))
            {

                testModels.Add(new GetTestSimpleModel
                {
                    Course = new GetCourseSimpleModel
                    {
                        Id = test.Course.Id,
                        Description = test.Course.Description,
                        Name = test.Course.Name
                    },
                    DateTime = test.ComplitionDate,
                    Id = test.Id
                });
            }

            return testModels;
        }
        public GetTestForStudentModel GetTestForStudentPassing(string sessionId, int testId)
        {
            var testResponseModel = new GetTestForStudentModel();
            var questions = new List<GetQuestionForStudentModel>();

            var userData = cachingService.GetUserAuthDataFromCache(sessionId);
            if (userData == null)
            {
                throw new Exception(string.Format("Сесія з id не була знайдена.", sessionId));
            }

            var userTest = uow.UsersTests.Get(ut => ut.TestId == testId && ut.UserId == userData.UserId);
            if (userTest == null)
            {
                throw new Exception("Ви не є підписником тестування, яке намагалися пройти!");
            }

            var testInfo = uow.Tests.Get(t => t.Id == testId, include: t => t.Include(t => t.Course));

            var questionsInfo = uow.Questions.GetSeveral(q => q.TestId == testId);
            if (questionsInfo == null || questionsInfo.Count == 0)
            {
                throw new Exception("Тестування не містить питань. Можливо, сталася помилка при його створенні.");
            }

            foreach (var question in questionsInfo)
            {
                questions.Add(new GetQuestionForStudentModel
                {
                    QuestionId = question.Id,
                    IsFileQuestion = question.IsFileQuestion,
                    IsOpenQuestion = question.IsOpenQuestion,
                    Title = question.Title
                });
            }

            foreach (var question in questions)
            {
                if (question.IsFileQuestion || question.IsOpenQuestion)
                {
                    testResponseModel.Questions.Add(question);
                    continue;
                }

                var responseOptions = uow.ResponseOptions.GetSeveral(ro => ro.QuestionId == question.QuestionId);

                if (responseOptions == null || responseOptions.Count == 0)
                {
                    throw new Exception("Тестове питання не містить варіантів відповідей. Можливо, під час створення питання Ви забули вказати прапор isOpen або isFile.");
                }

                foreach (var ro in responseOptions)
                {
                    question.ResponseOptions.Add(new GetResponseOptionForStudentModel
                    {
                        ResponseOptionId = ro.Id,
                        Value = ro.Value
                    });
                }

                testResponseModel.Questions.Add(question);
            }

            testResponseModel.CourseName = testInfo.Course.Name;
            testResponseModel.TestId = testInfo.Id;
            testResponseModel.Theme = testInfo.Theme;
            //TODO: Add time limit for test in DB
            testResponseModel.TimeLimitInMinutes = 30;
            testResponseModel.ComplitionDate = testInfo.ComplitionDate;

            return testResponseModel;
        }
        public IEnumerable<decimal> ProcessTestResultSaving(SavePassedTestResultsModel testResult)
        {
            var gradesForTest = new List<decimal>();
            var userData = cachingService.GetUserAuthDataFromCache(testResult.SessionId);

            foreach (var question in testResult.Questions)
            {
                answersService.SaveAnswers(question, userData.UserId, testResult.TestId);
                var gradeForQuestion = answersService.EvaluateAnswersForQuestion(question.QuestionId, userData.UserId, question.ChosenResponseOptionIds);

                gradesForTest.Add(gradeForQuestion);
            }

            return gradesForTest;
        }
      /*  public GetTestForEvaluationModel GetTestForEvaluationModel(int testId, int studentId)
        {

        }*/

        #endregion

        #region Private methods

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

        #endregion
    }
}

