using DataAccess;
using DataAccess.Entities.Answers;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;
using DiplomaServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiplomaServices.Services.TestServices
{
    public class AnswersService : IAnswersService
    {

        #region Fields

        private readonly IUnitOfWork uow;

        private readonly IQuestionService questionService;

        private const string MAIN_FOLDER_RELATIVE_PATH = @"..\FiledAnswers\";

        #endregion

        #region Public methods

        public AnswersService(IUnitOfWork uow,
            IQuestionService questionService)
        {
            this.uow = uow;
            this.questionService = questionService;
        }
        public void SaveAnswers(SavePassedTestResultQuestionModel questionWithAnswers, int userId, int testId)
        {
            var question = uow.Questions.Get(q => q.Id == questionWithAnswers.QuestionId);

            if (question.IsFileQuestion)
            {
                SaveFiledAnswer(userId, question.Id, testId, questionWithAnswers.FileName, questionWithAnswers.OpenOrFileAnswerValue);
            }
            else if (question.IsOpenQuestion)
            {
                SaveOpenAnswer(userId, question.Id, questionWithAnswers.OpenOrFileAnswerValue);
            }
            else
            {
                SaveUsualAnswer(userId, question.Id, questionWithAnswers.ChosenResponseOptionIds);
            }

            uow.Save();
        }

        public List<GetUserAnswerForEvaluationModel> GetUserAnswersForEvaluation(int questionId, int userId)
        {
            var userAnswersResponseList = new List<GetUserAnswerForEvaluationModel>();

            var userAnswers = uow.UserAnswers.GetSeveral(ua => ua.QuestionId == questionId && ua.UserId == userId,
                include: ua => ua
                .Include(ua => ua.Question)
                .Include(ua => ua.ResponseOption));

            if (userAnswers == null)
            {
                return new List<GetUserAnswerForEvaluationModel>();
            }

            var question = uow.Questions.Get(q => q.Id == questionId);

            if (question.IsFileQuestion)
            {
                userAnswersResponseList.Add(new GetUserAnswerForEvaluationModel
                {
                    Value = GetFiledAnswer(userId, questionId)
                });

                return userAnswersResponseList;
            }
            else
            {

                foreach (var userAnswer in userAnswers)
                {
                    var userAnswerResponse = new GetUserAnswerForEvaluationModel { Value = userAnswer.Value };

                    if (!question.IsOpenQuestion)
                    {
                        userAnswerResponse.ResponseOptionId = userAnswer.ResponseOptionId;
                    }

                    userAnswersResponseList.Add(userAnswerResponse);
                }

                return userAnswersResponseList;
            }
        }
        public void EvaluateAnswersForQuestion(int questionId, int userId, List<int> chosenROIds, decimal? openQuestionGrade)
        {
            var question = uow.Questions.Get(q => q.Id == questionId);
            var userAnswer = uow.UserAnswers.Get(ua => ua.QuestionId == questionId);

            if ((question.IsFileQuestion || question.IsOpenQuestion) && openQuestionGrade != null)
            {
                uow.TestResults.Create(new TestResult
                {
                    UserAnswerId = userAnswer.Id,
                    SummaryGrade = openQuestionGrade.Value
                });
            }
            else
            if(question.IsFileQuestion || question.IsOpenQuestion)
            {
                uow.TestResults.Create(new TestResult
                {
                    UserAnswerId = userAnswer.Id
                });
            }
            else
            {
                var grade = questionService.CountGradeForQuestion(questionId, chosenROIds);

                uow.TestResults.Create(new TestResult
                {
                    UserAnswerId = userAnswer.Id,
                    SummaryGrade = grade
                });
            }

            uow.Save();
        }
        public string GetFiledAnswer(int userId, int questionId)
        {
            var userAnswer = uow.UserAnswers.Get(ua => ua.QuestionId == questionId && ua.UserId == userId);
            if (userAnswer == null)
            {
                return string.Empty;
            }

            var fileEncodedToBase64 = Convert.ToBase64String(File.ReadAllBytes(userAnswer.Value));

            return fileEncodedToBase64;
        }

        #endregion 

        #region Private methods

        private void SaveFiledAnswer(int userId, int questionId, int testId, string fileName, string value)
        {
            var generatedFileName = CreateFileName(userId, testId, questionId);
            var fileExtension = Path.GetExtension(fileName);

            if (!Directory.Exists(MAIN_FOLDER_RELATIVE_PATH))
            {
                Directory.CreateDirectory(MAIN_FOLDER_RELATIVE_PATH);
            }

            var filePath = string.Format(@"{0}{1}{2}", MAIN_FOLDER_RELATIVE_PATH, generatedFileName, fileExtension);

            var spl = value.Split('/')[1];
            var format = spl.Split(';')[0];
            var validBase64String = value.Replace($"data:image/{format};base64,", string.Empty);

            File.Create(filePath).Close();
            File.WriteAllBytes(filePath, Convert.FromBase64String(validBase64String));

            uow.UserAnswers.Create(new UserAnswer
            {
                QuestionId = questionId,
                UserId = userId,
                Value = filePath
            });

            uow.Save();
        }
        private void SaveOpenAnswer(int userId, int questionId, string answerValue)
        {
            var answer = new UserAnswer
            {
                UserId = userId,
                QuestionId = questionId,
                Value = answerValue
            };

            uow.UserAnswers.Create(answer);

            uow.Save();
        }
        private void SaveUsualAnswer(int userId, int questionId, List<int> chosenROIds)
        {
            foreach (var roId in chosenROIds)
            {
                var responseOptionById = uow.ResponseOptions.Get(ro => ro.Id == roId);
                if (responseOptionById != null)
                {
                    var userAnswer = new UserAnswer
                    {
                        QuestionId = questionId,
                        ResponseOptionId = responseOptionById.Id,
                        UserId = userId,
                        Value = responseOptionById.Value
                    };

                    uow.UserAnswers.Create(userAnswer);
                }
            }
        }
        private string CreateFileName(int userId, int testId, int questionId)
        {
            return string.Format("{0}_{1}_{2}", userId, testId, questionId);
        }

        #endregion
    }
}
