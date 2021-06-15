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
        #region Fields

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        #endregion

        #region Public methods

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
            if (!model.IsFileQuestion && !model.IsOpenQuestion && fullResponseOptions != null)
            {
                CreateRightAnswersForTheQuestion(fullResponseOptions);
            }
        }

        public List<GetQuestionModel> GetQuestionsForTheTest(int testId)
        {
            var questions = uow.Questions.GetSeveral(q => q.TestId == testId);
            var responseQuestionModels = new List<GetQuestionModel>();

            foreach (var question in questions)
            {
                var responseOptionsByQuestion = uow.ResponseOptions.GetSeveral(rp => rp.QuestionId == question.Id);
                var getResponseOptions = new List<GetResponseOptionModel>();
                var rightResponseOptionIds = GetRightResponseOptionIdsForQuestion(question.Id);

                foreach (var item in responseOptionsByQuestion)
                {
                    getResponseOptions.Add(new GetResponseOptionModel { Value = item.Value });
                }

                var responseQuestionModel = new GetQuestionModel()
                {
                    Id = question.Id,
                    Title = question.Title,
                    ResponseOptions = getResponseOptions,
                    IsFileQuestion = question.IsFileQuestion,
                    IsOpenQuestion = question.IsOpenQuestion,
                    MaxGrade = CountGradeForQuestion(question.Id, rightResponseOptionIds)
                };

                responseQuestionModels.Add(responseQuestionModel);
            }

            return responseQuestionModels;
        }
        public List<GetDetailedQuestionModel> GetDetailedQuestionsForTheTest(int testId)
        {
            var questions = uow.Questions.GetSeveral(q => q.TestId == testId);
            var responseQuestionModels = new List<GetDetailedQuestionModel>();

            foreach (var question in questions)
            {
                var responseOptionsByQuestion = uow.ResponseOptions.GetSeveral(rp => rp.QuestionId == question.Id);
                var getResponseOptions = new List<GetDetailedResponseOptionModel>();
                var rightResponseOptionIds = GetRightResponseOptionIdsForQuestion(question.Id);

                foreach (var item in responseOptionsByQuestion)
                {
                    getResponseOptions.Add(new GetDetailedResponseOptionModel { Value = item.Value, ResponseOptionId = item.Id });
                }

                var responseQuestionModel = new GetDetailedQuestionModel()
                {
                    QuestionId = question.Id,
                    Title = question.Title,
                    ResponseOptions = getResponseOptions,
                    IsFileQuestion = question.IsFileQuestion,
                    IsOpenQuestion = question.IsOpenQuestion,
                    MaxGrade = CountGradeForQuestion(question.Id, rightResponseOptionIds)
                };

                responseQuestionModels.Add(responseQuestionModel);
            }

            return responseQuestionModels;
        }
        public int CountNumberOfValidAnswers(CreateQuestionModel model)
        {
            var numberOfValidAnswers = 0;

            foreach (var responseOption in model.ResponseOptions)
            {
                if (responseOption.IsValid)
                {
                    numberOfValidAnswers++;
                }
            }

            return numberOfValidAnswers;
        }
        public decimal CountGradeForQuestion(int questionId, List<int> chosenROIds)
        {
            decimal grade = 0;

            var question = uow.Questions.Get(q => q.Id == questionId);

            if (!question.IsFileQuestion && !question.IsOpenQuestion)
            {
                if (chosenROIds.Count == 0)
                {
                    return grade;
                }
                else
                {
                    foreach (var roId in chosenROIds)
                    {
                        var rightSimpleAnswer = uow.RightSimpleAnswers.Get(rsa => rsa.ResponseOptionId == roId);

                        if (rightSimpleAnswer != null)
                        {
                            grade += rightSimpleAnswer.Grade;
                        }
                    }
                }
            }

            return grade;
        }
        public List<int> GetRightResponseOptionIdsForQuestion(int questionId)
        {
            var responseOptions = uow.ResponseOptions.GetSeveral(ro => ro.QuestionId == questionId);
            var rightResponseOptionIds = new List<int>();

            foreach (var ro in responseOptions)
            {
                var rightSimpleAnswers = uow.RightSimpleAnswers.GetSeveral(rsa => rsa.ResponseOptionId == ro.Id);
                foreach (var rsa in rightSimpleAnswers)
                {
                    rightResponseOptionIds.Add(rsa.ResponseOptionId);
                }
            }

            return rightResponseOptionIds;
        }

        #endregion

        #region Private methods
        private IEnumerable<CreateResponseOptionModelWithId> CreateResponseOptionsForTheQuestion(CreateQuestionModel model, int questionId)
        {
            List<CreateResponseOptionModelWithId> fullResponseOptions = new List<CreateResponseOptionModelWithId>();

            if (!model.IsFileQuestion && !model.IsOpenQuestion && model.ResponseOptions.Count > 0)
            {
                foreach (var responseOption in model.ResponseOptions)
                {
                    decimal grade = 0;

                    var numberOfValidAnswers = CountNumberOfValidAnswers(model);

                    if (numberOfValidAnswers > 1)
                    {
                        grade = model.Grade / numberOfValidAnswers;
                    }
                    else
                    {
                        grade = model.Grade;
                    }

                    var responseOptionEntity = mapper.Map<CreateResponseOptionModel, ResponseOption>(responseOption);
                    responseOptionEntity.QuestionId = questionId;

                    uow.ResponseOptions.Create(responseOptionEntity);
                    uow.Save();

                    fullResponseOptions.Add(new CreateResponseOptionModelWithId
                    {
                        Id = responseOptionEntity.Id,
                        Value = responseOptionEntity.Value,
                        IsValid = responseOption.IsValid,
                        Grade = grade
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

            foreach (var rightAnswer in rightAnswersToCreate)
            {
                uow.RightSimpleAnswers.Create(mapper.Map<CreateRightAnswerModel, RightSimpleAnswer>(rightAnswer));
                uow.Save();
            }
        }

        #endregion

    }
}
 