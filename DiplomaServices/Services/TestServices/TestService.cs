using DataAccess;
using DataAccess.Entities.Answers;
using DataAccess.Entities.ManyToManyEntities;
using DataAccess.Entities.TestEntities;
using DiplomaServices.Interfaces;
using DiplomaServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomaServices.Services.TestServices
{
    public class TestService : ITestService
    {
        #region Fields

        private readonly IUnitOfWork uow;

        private readonly ICourseService courseService;

        private readonly IQuestionService questionService;

        private readonly IEmailService emailService;

        private readonly IUserService userService;

        private readonly IAnswersService answersService;

        private readonly CachingService cachingService;

        #endregion

        #region Public methods

        public TestService(IUnitOfWork uow,
            ICourseService courseService,
            IQuestionService questionService,
            IEmailService emailService,
            IUserService userService,
            IAnswersService answersService,
            IMemoryCache cache)
        {
            this.uow = uow;
            this.courseService = courseService;
            this.questionService = questionService;
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

                    questionService.CreateQuestion(question);
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
                    Id = test.Id,
                    Theme = test.Theme,
                    IsActive = test.ComplitionDate>=DateTime.Today
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

                question.NumberOfRightAnswers = CountNumberOfRightAnswersPerQuestion(question.QuestionId);

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
        public void ProcessTestResultSaving(SavePassedTestResultsModel testResult)
        {
            var userData = cachingService.GetUserAuthDataFromCache(testResult.SessionId);

            foreach (var question in testResult.Questions)
            {
                answersService.SaveAnswers(question, userData.UserId, testResult.TestId);

                answersService.EvaluateAnswersForQuestion(question.QuestionId, userData.UserId, question.ChosenResponseOptionIds, null);
            }
        }
        public GetTestForEvaluationModel GetTestForEvaluation(string sessionId, int testId, int studentId)
        {
            var test = uow.Tests.Get(t => t.Id == testId);
            var testResponse = new GetTestForEvaluationModel();
            var questionsForEvaluation = new List<GetQuestionForEvaluationModel>();

            if (test == null)
            {
                throw new Exception(string.Format("Тестування з id {0} не було знайдено.", testId));
            }

            var questions = questionService.GetQuestionsForTheTest(testId);

            if (questions == null || questions.Count == 0)
            {
                throw new Exception("Тест не містить питань! Порожній тест.");
            }

            foreach (var question in questions)
            {
                var rightResponseOptionIds = questionService.GetRightResponseOptionIdsForQuestion(question.Id);
                decimal? maxGrade = null;

                if (!question.IsFileQuestion && !question.IsOpenQuestion)
                {
                    maxGrade = questionService.CountGradeForQuestion(question.Id, rightResponseOptionIds);
                }

                questionsForEvaluation.Add(new GetQuestionForEvaluationModel
                {
                    QuestionId = question.Id,
                    IsFileQuestion = question.IsFileQuestion,
                    IsOpenQuestion = question.IsOpenQuestion,
                    QuestionTitle = question.Title,
                    AnswerMaxGrade = maxGrade
                });
            }

            foreach (var question in questionsForEvaluation)
            {
                var chosenResponseOptionIds = new List<int>();
                var userAnswers = answersService.GetUserAnswersForEvaluation(question.QuestionId, studentId);

                if (!question.IsFileQuestion && !question.IsOpenQuestion)
                {
                    foreach (var ua in userAnswers)
                    {
                        chosenResponseOptionIds.Add(ua.ResponseOptionId.Value);
                    }

                    question.SummaryGrade = questionService.CountGradeForQuestion(question.QuestionId, chosenResponseOptionIds);
                }
                    question.UserAnswers = userAnswers;
               
                testResponse.Questions.Add(question);
            }

            var student = uow.Users.Get(u => u.Id == studentId);

            testResponse.ComplitionDate = test.ComplitionDate.ToString();
            testResponse.StudentId = studentId;
            testResponse.StudentLogin = student.Login;
            testResponse.TestId = test.Id;
            testResponse.Theme = test.Theme;

            return testResponse;
        }
        public void SaveTestAfterEvaluation(SaveTestAfterEvaluationModel model)
        {
            foreach (var questionResult in model.QuestionResults)
            {
                answersService.EvaluateAnswersForQuestion(questionResult.QuestionId, model.StudentId, null, questionResult.Grade);
            }
        }
        public List<GetDetailedTestModel> GetDetailedTestsListByStudentId(int studentId)
        {
            var testsByStudentIdResponse = new List<GetDetailedTestModel>();

            var student = uow.Users.Get(u => u.Id == studentId);
            if(student == null)
            {
                throw new Exception("Користувач не знайдений!");
            }

            var studentTests = uow.UsersTests.GetSeveral(ut => ut.UserId == studentId);
            if(studentTests == null)
            {
                return null;
            }

            foreach (var test in studentTests)
            {
                var testEntity = uow.Tests.Get(t => t.Id == test.TestId);
                var questionsForTest = questionService.GetDetailedQuestionsForTheTest(testEntity.Id);
                var questionsResponse = new List<CreateQuestionModel>();

                var testResponse = new GetDetailedTestModel
                {
                    Theme = testEntity.Theme,
                    TestId = testEntity.Id,
                    DateTime = testEntity.ComplitionDate,
                    ExpireTime = 30.ToString(),
                    Questions = questionService.GetDetailedQuestionsForTheTest(testEntity.Id)
                };

                testsByStudentIdResponse.Add(testResponse);
            }

            return testsByStudentIdResponse;
        }
        public GetTestResultFullyEvaluatedModel GetTestResultFullyEvaluated(int studentId, int testId)
        {
            var test = uow.Tests.Get(t => t.Id == testId,
                include: t => t.Include(t => t.Course));
            var questions = uow.Questions.GetSeveral(q => q.TestId == testId);
            var gradesPerQuestion = new List<decimal>();
            var questionsForTest = new List<GetEvaluatedQuestionModel>();

            foreach (var question in questions)
            {
                //answers given by user
                var userAnswers = uow.UserAnswers.GetSeveral(ua => ua.QuestionId == question.Id);

                if (!question.IsFileQuestion && !question.IsOpenQuestion)
                {
                    //right answers
                    var rightResponseOptionIds = questionService.GetRightResponseOptionIdsForQuestion(question.Id);
                    //max grade
                    var maxQuestionGrade = questionService.CountGradeForQuestion(question.Id, rightResponseOptionIds);
                    //all possible response options
                    var allResponseOptionsForQuestion = uow.ResponseOptions.GetSeveral(ro => ro.QuestionId == question.Id);
                  
                    var chosenROids = new List<int>();
                    var responseOptionsToAdd = new List<GetEvaluatedResponseOptions>();

                    //Count summary grade for question
                    decimal questionSummaryGrade = 0;
                    foreach (var ua in userAnswers)
                    {
                        var testResult = uow.TestResults.Get(tr => tr.UserAnswerId == ua.Id);
                        questionSummaryGrade += testResult.SummaryGrade;

                        if (ua.ResponseOptionId != null)
                        {
                            chosenROids.Add(ua.ResponseOptionId.Value);
                        }
                    }

                    //Adding info about chosen answers
                    foreach (var ro in allResponseOptionsForQuestion)
                    {
                        var roToAdd = new GetEvaluatedResponseOptions();

                        foreach (var roChosen in chosenROids)
                        {
                            if (ro.Id == roChosen)
                            {
                                roToAdd.IsChosen = true;
                            }

                            roToAdd.ResponseOptionId = ro.Id;
                            roToAdd.Value = ro.Value;
                        }

                        foreach (var roValid in rightResponseOptionIds)
                        {
                            if (ro.Id == roValid)
                            {
                                roToAdd.IsValid = true;
                            }
                        }

                        responseOptionsToAdd.Add(roToAdd);
                    }

                    gradesPerQuestion.Add(questionSummaryGrade);

                    var questionToAdd = new GetEvaluatedQuestionModel
                    {
                        IsFileQuestion = question.IsFileQuestion,
                        IsOpenQuestion = question.IsOpenQuestion,
                        MaxGrade = maxQuestionGrade,
                        SummaryGrade = questionSummaryGrade,
                        ResponseOptions = responseOptionsToAdd,
                        Title = question.Title,
                        QuestionId = question.Id
                    };

                    questionsForTest.Add(questionToAdd);
                }
                else
                if(question.IsFileQuestion)
                {
                    var userAnswerValue = answersService.GetFiledAnswer(studentId, question.Id);
                    var testResult = uow.TestResults.Get(tr => tr.UserAnswerId == userAnswers[0].Id);
                    gradesPerQuestion.Add(testResult.SummaryGrade);

                    var questionToAdd = new GetEvaluatedQuestionModel
                    {
                        IsFileQuestion = question.IsFileQuestion,
                        IsOpenQuestion = question.IsOpenQuestion,
                        Title = question.Title,
                        QuestionId = question.Id,
                        ValueForFiledOrOpenQuestion = userAnswerValue,
                        SummaryGrade = testResult.SummaryGrade
                    };

                    questionsForTest.Add(questionToAdd);
                }
                else
                {
                    var testResult = uow.TestResults.Get(tr => tr.UserAnswerId == userAnswers[0].Id);
                    gradesPerQuestion.Add(testResult.SummaryGrade);

                    var questionToAdd = new GetEvaluatedQuestionModel
                    {
                        IsFileQuestion = question.IsFileQuestion,
                        IsOpenQuestion = question.IsOpenQuestion,
                        Title = question.Title,
                        QuestionId = question.Id,
                        ValueForFiledOrOpenQuestion = userAnswers[0].Value,
                        SummaryGrade = testResult.SummaryGrade
                    };

                    questionsForTest.Add(questionToAdd);
                }
            }

            return new GetTestResultFullyEvaluatedModel
            {
                CourseName = test.Course.Name,
                TestId = test.Id,
                Questions = questionsForTest,
                Theme = test.Theme,
                SummaryGrade = gradesPerQuestion.Sum()
            };
        }

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
        private int CountNumberOfRightAnswersPerQuestion(int questionId)
        {
            int numberOfRightAnswers = 0;

            var question = uow.Questions.Get(q => q.Id == questionId);
            var responseOptions = uow.ResponseOptions.GetSeveral(ro => ro.QuestionId == question.Id);
            var allRightAnswers = uow.RightSimpleAnswers.GetAll();

            foreach (var rightAsnwer in allRightAnswers)
            {
                foreach (var ro in responseOptions)
                {
                    if (rightAsnwer.ResponseOptionId == ro.Id)
                    {
                        numberOfRightAnswers++;
                        break;
                    }
                }    
            }

            return numberOfRightAnswers;
        }

        #endregion
    }
}

