using DataAccess;
using DataAccess.Entities.Answers;
using DataAccess.Entities.TestEntities;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;
using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Services.TestServices
{
    public class QuestionService : IQuestionService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        #endregion

        public QuestionService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        public void CreateQuestion(CreateQuestionModel model)
        {
            //Create Question
            var question = mapper.Map<CreateQuestionModel, Question>(model);
            uow.Questions.Create(question);
            uow.Save();

            //Create Response Options
            var fullResponseOptions = CreateResponseOptionsForTheQuestion(model, question.Id);

            //Create RightAnswers
            if(!model.IsFileQuestion && !model.IsOpenQuestion && fullResponseOptions != null)
            {
                CreateRightAnswersForTheQuestion(fullResponseOptions);
            }
    }

        public List<GetQuestionModel> GetQuestionsForTheTest(int testId)
        {
            var questions = uow.Questions.GetByPredicate(q => q.TestId == testId);
            var responseQuestionModels = new List<GetQuestionModel>();

            foreach (var question in questions)
            {
                var responseOptionsByQuestion = uow.ResponseOptions.GetByPredicate(rp => rp.QuestionId == question.Id);
                var getResponseOptions = new List<GetResponseOptionModel>();

                foreach (var item in responseOptionsByQuestion)
                {
                    getResponseOptions.Add(new GetResponseOptionModel { Value = item.Value });
                }

                var responseQuestionModel = new GetQuestionModel()
                {
                    Title = question.Title,
                    ResponseOptions = getResponseOptions,
                    IsFileQuestion = question.IsFileQuestion,
                    IsOpenQuestion = question.IsOpenQuestion
                };

                responseQuestionModels.Add(responseQuestionModel);
            }
            
            return responseQuestionModels;
        }

        private IEnumerable<CreateResponseOptionModelWithId> CreateResponseOptionsForTheQuestion(CreateQuestionModel model, int questionId)
        {
            List<CreateResponseOptionModelWithId> fullResponseOptions = new List<CreateResponseOptionModelWithId>();

            if (!model.IsFileQuestion && !model.IsOpenQuestion && model.ResponseOptions.Count > 0)
            {
                foreach (var responseOption in model.ResponseOptions)
                {
                    var responseOptionEntity = mapper.Map<CreateResponseOptionModel, ResponseOption>(responseOption);
                    responseOptionEntity.QuestionId = questionId;

                    uow.ResponseOptions.Create(responseOptionEntity);
                    uow.Save();

                    fullResponseOptions.Add(new CreateResponseOptionModelWithId
                    {
                        Id = responseOptionEntity.Id,
                        Value = responseOptionEntity.Value,
                        IsValid = responseOption.IsValid,
                        Grade = responseOption.Grade
                    });
                }

                return fullResponseOptions;
            }
            else
            {
                return null;
            }
        }

        private void CreateRightAnswersForTheQuestion(IEnumerable<CreateResponseOptionModelWithId> fullResponseOptions)
        {
            List<CreateRightAnswerModel> rightAnswersToCreate = new List<CreateRightAnswerModel>();

            foreach (var fullResponseOption in fullResponseOptions)
            {
                if (fullResponseOption.IsValid)
                {
                    var rightAnswer = new CreateRightAnswerModel() { ResponseOptionId = fullResponseOption.Id, Grade = fullResponseOption.Grade };
                    rightAnswersToCreate.Add(rightAnswer);
                }
            }

            foreach(var rightAnswer in rightAnswersToCreate)
            {
                uow.RightSimpleAnswers.Create(mapper.Map<CreateRightAnswerModel, RightSimpleAnswer>(rightAnswer));
                uow.Save();
            }
        }
    }
}
