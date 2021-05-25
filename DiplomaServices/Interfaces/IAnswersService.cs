using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface IAnswersService
    {
        public void SaveAnswers(SavePassedTestResultQuestionModel questionWithAnswers, int userId, int testId);
        public decimal EvaluateAnswersForQuestion(int questionId, int userId, List<int> chosenROIds);
    }
}
