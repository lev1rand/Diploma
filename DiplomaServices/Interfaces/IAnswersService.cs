using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface IAnswersService
    {
        public void SaveAnswers(SavePassedTestResultQuestionModel questionWithAnswers, int userId, int testId);
        public void EvaluateAnswersForQuestion(int questionId, int userId, List<int> chosenROIds, decimal? openQuestionGrade);
        public List<GetUserAnswerForEvaluationModel> GetUserAnswersForEvaluation(int questionId, int userId);
        public string GetFiledAnswer(int userId, int questionId);
    }
}
