using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface IQuestionService
    {
        public void CreateQuestion(CreateQuestionModel model);
        public List<GetQuestionModel> GetQuestionsForTheTest(int testId);
        public decimal CountGradeForQuestion(int questionId, List<int> chosenROIds);
        public List<int> GetRightResponseOptionIdsForQuestion(int questionId);
        public List<GetDetailedQuestionModel> GetDetailedQuestionsForTheTest(int testId);
    }
}
