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

        #region private members

        private readonly IUnitOfWork uow;

        #endregion

        public AnswersService(IUnitOfWork uow)
        {
            this.uow = uow;
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
        }


        public decimal EvaluateAnswersForQuestion(int questionId, int userId, List<int> chosenROIds)
        {
            var grade = CountGradeForQuestion(questionId, chosenROIds);

            return grade;
        }

        private void SaveFiledAnswer(int userId, int questionId, int testId, string fileName, string value)
        {
            var generatedFileName = CreateFileName(userId, testId, questionId);
            var fileExtension = Path.GetExtension(fileName);
            var filePath = string.Format(@"..\Diploma\FiledAnswers\{0}{1}", generatedFileName, fileExtension);

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

        private decimal CountGradeForQuestion(int questionId, List<int>chosenROIds)
        {
            decimal grade = 0;

            var question = uow.Questions.Get(q => q.Id == questionId);

            if (!question.IsFileQuestion && !question.IsOpenQuestion)
            {
                if(chosenROIds.Count == 0)
                {
                    return grade;
                }
                else
                {
                    foreach (var roId in chosenROIds)
                    {
                        var rightSimpleAnswer = uow.RightSimpleAnswers.Get(rsa => rsa.ResponseOptionId == roId);

                        if (rightSimpleAnswer == null)
                        {
                            return grade;
                        }
                        else
                        {
                            grade += rightSimpleAnswer.Grade;
                        }
                    }
                }
            }

            return grade;
        }
    }
}
