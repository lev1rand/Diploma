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
        public void SaveAnswers(List<SaveUserAnswersModel> answers, int userId)
        {
            foreach (var answer in answers)
            {
                var question = uow.Questions.Get(q => q.Id == answer.QuestionId);

                if (question.IsFileQuestion)
                {
                    SaveFiledAnswer(new SaveUserAnswerDetailedModel
                    {
                        Answer = answer,
                        Question = question,
                        UserId = userId
                    });
                }
                else
                {
                    SaveNonFiledAnswer(new SaveUserAnswerDetailedModel
                    {
                        Answer = answer,
                        Question = question,
                        UserId = userId
                    });
                }
            }
        }

        public void EvaluateAnswers(List<SaveUserAnswersModel> answers, int userId)
        {
            foreach (var answer in answers)
            {
                var answerByQuestionAndUser =
                    uow.UserAnswers.Get(ua =>
                    ua.QuestionId == answer.QuestionId &&
                    ua.UserId == userId,
                    include: ua => ua.Include(ua => ua.Question));


                var question = uow.Questions.Get(q => q.Id == answer.QuestionId);

                if (!question.IsFileQuestion && !question.IsOpenQuestion)
                {
                    var grade = CountGradeForAnswer(answerByQuestionAndUser);

                    var testResult = new TestResult
                    {
                        UserAnswerId = answerByQuestionAndUser.Id,
                        SummaryGrade = grade
                    };
                }
            }
        }

        private void SaveFiledAnswer(SaveUserAnswerDetailedModel answerModel)
        {
            var fileName = CreateFileName(answerModel.UserId, answerModel.Question.TestId, answerModel.Question.Id);
            var fileExtension = Path.GetExtension(answerModel.Answer.FileName);
            var filePath = string.Format(@"..\Diploma\FiledAnswers\{0}{1}", fileName, fileExtension);

            var spl = answerModel.Answer.Value.Split('/')[1];
            var format = spl.Split(';')[0];
            var validBase64String = answerModel.Answer.Value.Replace($"data:image/{format};base64,", string.Empty);

            File.Create(filePath).Close();
            File.WriteAllBytes(filePath, Convert.FromBase64String(validBase64String));

            uow.UserAnswers.Create(new UserAnswer
            {
                QuestionId = answerModel.Question.Id,
                UserId = answerModel.UserId,
                Value = filePath
            });

            uow.Save();
        }

        private void SaveNonFiledAnswer(SaveUserAnswerDetailedModel answerModel)
        {
            var answer = new UserAnswer
            {
                UserId = answerModel.UserId,
                QuestionId = answerModel.Question.Id,
                Value = answerModel.Answer.Value
            };

            uow.UserAnswers.Create(answer);

            uow.Save();
        }

        private string CreateFileName(int userId, int testId, int questionId)
        {
            return string.Format("{0}_{1}_{2}", userId, testId, questionId);
        }

        private decimal CountGradeForAnswer(UserAnswer answer)
        {
            //Get ResponseOptions of specific Question
            var responseOptions = uow.ResponseOptions.GetSeveral(ro =>
            ro.QuestionId == answer.QuestionId,
            include: ro => ro.Include(ro => ro.Question));

            //Find whether value of Answer corresponds with Value of ResponseOption
            var responseOption = responseOptions.Find(ro => ro.Value == answer.Value);

            //If not found - return 0 grade
            if (responseOption == null)
            {
                return 0;
            }
            else
            {
                //Else we try to find corresponding ResponseOption in RightSimpleAnswers
                var rightSimpleAnswer = uow.RightSimpleAnswers.Get(rsa => rsa.ResponseOptionId == responseOption.Id);

                //If not found - return 0 grade
                if (rightSimpleAnswer == null)
                {
                    return 0;
                }
                else
                {
                    //Else get grade and return it
                    return rightSimpleAnswer.Grade;
                }
            }
        }
    }
}
